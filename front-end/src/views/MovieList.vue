<template>
  <div class="movie-list">
    <el-carousel height="150px">
      <el-carousel-item>
        <img class="slider--item" src="../assets/icon/film-image/poster.jpg" />
      </el-carousel-item>
    </el-carousel>
    <div class="search-input">
      <m-text-input
        class="input"
        individualClass="text__input-icon-start"
        placeholder="Tìm kiếm theo tên"
        v-model="this.inputSearch"
        @keydown.enter="
          () => this.getMovie(this.inputSearch, this.selectSearch)
        "
      ></m-text-input>
      <m-combo-box
        v-model="this.selectSearch"
        :dataSelect="this.optionsSearch"
        :filterable="true"
        placeholder="Chọn"
      ></m-combo-box>
    </div>
    <div class="card-list">
      <t-card
        v-for="movie in this.movieList"
        :key="movie.movieId"
        @showModal="showModal"
        :imageLink="movie.image"
        :movieTitle="movie.movieTitle"
        :movieDuration="movie.duration"
        :movieDate="movie.yearOfRelease"
        :movieId="movie.movieId"
        @click="this.movieName = movie.movieTitle"
      ></t-card>
    </div>
  </div>

  <t-modal v-if="isShowModal" @closeModal="closeModal">
    <BookingDetail :movieName="this.movieName" @showToastSuccess="showToastSuccess" @closeModal="closeModal"></BookingDetail>
  </t-modal>
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

<script module>
import BookingDetail from "./BookingDetail.vue";
import instance from "../common/instance";
import { delay } from "@/common/common";
export default {
  name: "MovieList",
  components: {
    BookingDetail,
  },
  data() {
    return {
      isShowToastSuccess: false,
      isShowToastError: false,
      labelToastSuccess: "",
      labelToastError: "",
      isShowModal: false,
      movieList: [],
      movieName: "",
      optionsSearch: [
        {
          label: "Phim đang chiếu",
          value: "1",
        },
        {
          label: "Phim sắp chiếu",
          value: "0",
        },
      ],
      inputSearch: "",
      selectSearch: "",
    };
  },
  async created() {
    await this.getMovie(this.inputSearch, "");
  },
  watch: {
    selectSearch: async function (newValue) {
      await this.getMovie(this.inputSearch, newValue);
    },
  },
  methods: {
    closeModal() {
      this.isShowModal = false;
    },
    showModal() {
      this.isShowModal = true;
    },
    async getMovie(inputSearch, selectSearch) {
      this.$store.commit("showMask");
      await instance
        .get(`Movie/Filter?movieTitle=${inputSearch}&isRelease=${selectSearch}`)
        .then(async (res) => {
          await delay(500);
          this.$store.commit("hideMask");
          this.movieList = res.data;
        })
        .catch((err) => console.log(err));
    },
    showToastSuccess(label) {
      this.isShowToastSuccess = true;
      this.labelToastSuccess = label;
      setTimeout(() => {
        this.isShowToastSuccess = false;
      }, 3000);
    },
    showToastError(label) {
      this.isShowToastError = true;
      this.labelToastError = label;
      setTimeout(() => {
        this.isShowToastError = false;
      }, 3000);
    }
  },
};
</script>

<style>
.movie-list {
  display: flex;
  flex-direction: column;
  flex: 1;
}
.card-list {
  display: flex;
  flex-wrap: wrap;
}

.movie-list .search-input {
  padding-left: 13px;
  padding-bottom: 20px;
  display: flex;
  column-gap: 24px;
  margin-top: 20px;
}

.search-input .combo-box {
  width: 200px !important;
}

.movie-list .combo-box .el-input__wrapper {
  height: 40px !important;
}

.movie-list .text__input {
  width: 400px;
  height: 40px;
}

.slider--item {
  width: 100%;
  height: 500px;
  object-fit: cover;
  object-position: top;
}

.el-carousel {
  height: 500px !important;
}

.el-carousel__container {
  height: 500px !important;
}
</style>
