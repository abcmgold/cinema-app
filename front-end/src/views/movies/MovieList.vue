<template>
  <div class="movie-list__navbar">
    <div class="movie-list__navbar--left">
      <m-text-input
        v-model="this.searchNameInput"
        individualClass="text__input-icon-start"
        placeholder="Tìm kiếm theo tên"
      ></m-text-input>
    </div>
    <div class="movie-list__navbar--right">
      <m-button
        individualClass="btn--primary"
        label="+ Thêm phim mới"
        @click="this.showAddForm"
      ></m-button>
      <m-button
        @click="this.deleteMovie"
        individualClass="btn--single"
        icon="icon--delete"
      ></m-button>
    </div>
  </div>
  <div class="table">
    <div ref="tableHeader" class="movie-list__table--header">
      <table>
        <thead>
          <tr>
            <th class="align-center" style="width: 50px">
              <div class="table-checkbox">
                <m-checkbox ref="checkbox-all" type="primary"></m-checkbox>
              </div>
            </th>
            <th class="align-center" style="width: 50px">STT</th>
            <th class="align-center" style="width: 400px">Tên phim</th>
            <th class="align-center" style="width: 200px">Đạo diễn</th>
            <th class="align-center" style="width: 200px">Diễn viên</th>
            <th class="align-center" style="width: 200px">Thời lượng(p)</th>
            <th class="align-center" style="width: 200px">Thể loại</th>
            <th style="width: 207px">Chức năng</th>
          </tr>
        </thead>
      </table>
    </div>
    <div ref="tableContent" class="movie-list__table" @scroll="handleScroller">
      <table>
        <tbody>
          <tr v-for="(movie, index) in movieList" :key="movie.movieId">
            <td class="align-center" style="width: 50px">
              <div class="table-checkbox">
                <m-checkbox
                  @click="clickOnCheckbox(index, movie.movieId)"
                  ref="checkbox"
                  type="primary "
                ></m-checkbox>
              </div>
            </td>
            <td class="align-center" style="width: 50px">{{ index + 1 }}</td>
            <td class="align-center" style="width: 400px">
              {{ movie.movieTitle }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ movie.movieDirector }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ movie.movieActors }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ movie.duration }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ movie.genreName }}
            </td>
            <td class="movie-list--actions" style="width: 200px">
              <m-button
                individualClass="btn--single"
                icon="icon--update"
                @click="this.openFormDetail(index)"
              ></m-button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <MovieForm
    v-if="this.isShowForm"
    @hideAddForm="this.hideAddForm"
    @updateMovie="this.updateMovie"
    @insertMovie="this.insertMovie"
    :genreObject="this.genreObject"
    :movieSelected="this.movieSelected"
    :formMode="this.formMode"
  ></MovieForm>
  <m-toast
    :label="this.labelToastSuccess"
    icon="icon--success"
    v-if="isShowToastSuccess"
  ></m-toast>
  <m-toast
    :label="this.labelToastError"
    icon="icon--error"
    v-if="isShowToastError"
  ></m-toast>
</template>

<script>
import instance from "../../common/instance";
import MovieForm from "./MovieForm.vue";
import debounce from "lodash/debounce";
export default {
  components: {
    MovieForm,
  },
  data() {
    return {
      movieList: [],
      genreObject: [],
      isShowForm: false,
      movieSelected: null,
      formMode: null,
      isShowToastSuccess: false,
      isShowToastError: false,
      labelToastSuccess: "",
      labelToastError: "",
      selectedMovieIds: [],
      searchNameInput: "",
    };
  },
  async created() {
    await this.getMovie();

    await this.getAllGenre();
  },
  watch: {
    searchNameInput: debounce(async function () {
      await this.getMovie();
    }, 1000),
  },

  methods: {
    showAddForm() {
      this.formMode = this.$_MISAEnum.formAdd;
      this.isShowForm = true;
    },
    hideAddForm() {
      this.isShowForm = false;
    },
    openFormDetail(index) {
      this.formMode = this.$_MISAEnum.formUpdate;
      this.isShowForm = true;
      this.movieSelected = this.movieList[index];
    },
    async updateMovie(movie) {
      let success = false;
      await instance
        .put(`Movie/${movie.movieId}`, movie)
        .then(() => {
          success = true;
        })
        .catch((err) => console.log(err));
      if (success == true) {
        this.hideAddForm();
        await this.getMovie();
        this.showToastSuccess("Chỉnh sửa thành công");
      }
    },
    async insertMovie(movie) {
      let success = false;
      await instance
        .post("Movie", movie)
        .then(() => {
          success = true;
        })
        .catch((err) => console.log(err));
      if (success) {
        this.hideAddForm();
        await this.getMovie();
        this.showToastSuccess("Thêm thành công");
      }
    },
    async getAllGenre() {
      await instance
        .get("Genre")
        .then((res) => {
          res.data.forEach((value) => {
            this.genreObject.push({
              id: value.genreId,
              value: value.genreName,
              label: value.genreName,
            });
          });
        })
        .catch((err) => console.log(err));
    },
    async getMovie() {
      this.$store.commit("showMask");
      await instance
        .get(`Movie/FilterByName?name=${this.searchNameInput}`)
        .then(async (res) => {
          await this.delay(500);
          this.$store.commit("hideMask");
          this.movieList = res.data;
        })
        .catch((err) => console.log(err));
    },
    async deleteMovie() {
      let success = false;
      const idsToDelete = this.selectedMovieIds.map((item) => item.toString());

      // Gửi yêu cầu DELETE với dữ liệu dạng JSON
      await instance
        .delete("Movie", { data: idsToDelete })
        .then(() => {
          success = true;
        })
        .catch((err) => console.log(err));

      if (success) {
        this.getMovie();
        this.showToastSuccess("Xóa thành công");
      }
    },

    showToastSuccess(label) {
      this.isShowToastSuccess = true;
      this.labelToastSuccess = label;
      // Ẩn sau 3s
      setTimeout(() => {
        this.isShowToastSuccess = false;
      }, 3000);
    },
    showToastError(label) {
      this.isShowToastError = true;
      this.labelToastError = label;
      // Ẩn sau 3s
      setTimeout(() => {
        this.isShowToastError = false;
      }, 3000);
    },
    clickOnCheckbox(index, id) {
      if (this.$refs["checkbox"][index].isChecked) {
        this.selectedMovieIds = this.selectedMovieIds.filter((idMovie) => {
          return id !== idMovie;
        });
      } else {
        this.selectedMovieIds.push(id);
      }
      this.$refs["checkbox"][index].isChecked =
        !this.$refs["checkbox"][index].isChecked;
      if (this.movieList.length === this.selectedMovieIds.length) {
        this.$refs["checkbox-all"].isChecked = true;
      } else {
        this.$refs["checkbox-all"].isChecked = false;
      }
    },

    delay(ms) {
      return new Promise((resolve) => setTimeout(resolve, ms));
    },

    handleScroller() {
      this.$refs.tableHeader.scrollLeft = this.$refs.tableContent.scrollLeft;
    },
  },
};
</script>

<style scoped>
@import url(../../css/views/movies/movie-list.css);
</style>
