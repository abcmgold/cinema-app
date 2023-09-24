<template>
  <div class="movie-list__navbar">
    <div class="movie-list__navbar--left">
      <m-text-input
        v-model="this.searchScreeningInput"
        individualClass="text__input-icon-start"
        placeholder="Tìm kiếm theo tên"
      ></m-text-input>
    </div>
    <div class="movie-list__navbar--right">
      <m-button
        individualClass="btn--primary"
        label="+ Thêm suất chiếu mới"
        @click="this.showForm"
      ></m-button>
      <m-button
        @click="this.deleteScreening"
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
            <th class="align-center" style="width: 200px">Phòng chiếu</th>
            <th class="align-center" style="width: 200px">Rạp</th>
            <th class="align-center" style="width: 200px">Thời gian bắt đầu</th>
            <th class="align-center" style="width: 200px">Giá vé</th>
            <th style="width: 207px">Chức năng</th>
          </tr>
        </thead>
      </table>
    </div>
    <div ref="tableContent" class="movie-list__table" @scroll="handleScroller">
      <table>
        <tbody>
          <tr
            v-for="(screening, index) in screeningList"
            :key="screening.screeningId"
          >
            <td class="align-center" style="width: 50px">
              <div class="table-checkbox">
                <m-checkbox
                  @click="clickOnCheckbox(index, screening.screeningId)"
                  ref="checkbox"
                  type="primary "
                ></m-checkbox>
              </div>
            </td>
            <td class="align-center" style="width: 50px">{{ index + 1 }}</td>
            <td class="align-center" style="width: 400px">
              {{ screening.movieTitle }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ screening.screenName }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ screening.cinemaName }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ formatedDateTime(screening.timeStart) }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ screening.ticketPrice }}
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
  <ScreeningForm
    v-if="this.isShowForm"
    @hideForm="this.hideForm"
    @updateScreening="this.updateScreening"
    @insertScreening="this.insertScreening"
    :movieList="this.movieList"
    :cinemaList="this.cinemaList"
    :screeningSelected="this.screeningSelected"
    :formMode="this.formMode"
  ></ScreeningForm>
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
import ScreeningForm from "./ScreeningForm.vue";
import { delay, formatDateTime } from '@/common/common';
import debounce from "lodash/debounce";
export default {
  name: "ScreeningList",
  components: {
    ScreeningForm,
  },
  data() {
    return {
      screeningList: [],
      movieList: [],
      cinemaList: [],
      selectedScreeningIds: [],
      isShowForm: false,
      formMode: "",
      screeningSelected: null,
      isShowToastSuccess: false,
      isShowToastError: false,
      labelToastSuccess: "",
      labelToastError: "",
      searchScreeningInput: ""
    };
  },
  async created() {
    await this.getScreeningList();
  },
  watch: {
    searchScreeningInput: debounce(async function () {
      await this.getScreeningList();
    }, 1000),
  },
  methods: {
    async getScreeningList() {
      this.$store.commit("showMask");
      await instance
        .get(`Screening/AdminFilterByName?name=${this.searchScreeningInput}`)
        .then(async (res) => {
          await delay(500);
          this.$store.commit("hideMask");
          this.screeningList = res.data;
        })
        .catch((err) => console.log(err));
    },
    async getCinemaList() {
      await instance
        .get("Cinema")
        .then((res) => {
          res.data.forEach((value) => {
            this.cinemaList.push({
              id: value.cinemaId,
              value: value.cinemaName,
              label: value.cinemaName,
            });
          });
        })
        .catch((err) => console.log(err));
    },
    async getMovieList() {
      await instance
        .get("Movie")
        .then((res) => {
          res.data.forEach((value) => {
            this.movieList.push({
              id: value.movieId,
              value: value.movieTitle,
              label: value.movieTitle,
            });
          });        })
        .catch((err) => console.log(err));
    },
    async updateScreening(screening) {
      screening.timeStart = this.convertTime(screening.timeStart)
      screening.timeEnd = this.convertTime(screening.timeEnd)
      let success = false;
      console.log(screening)
      await instance
        .put(`Screening/${screening.screeningId}`, screening)
        .then(() => {
          success = true;
        })
        .catch((err) => console.log(err));
      if (success == true) {
        this.hideForm();
        await this.getScreeningList();
        this.showToastSuccess("Chỉnh sửa thành công");
      }
    },
    async insertScreening(screening) {
      let success = false;
      await instance
        .post("Screening", screening)
        .then(() => {
          success = true;
        })
        .catch((err) => {
          this.showToastError(err.response.data);
        });
      if (success) {
        this.hideForm();
        await this.getScreeningList();
        this.showToastSuccess("Thêm thành công");
      }
    },
    async showForm() {
      await this.getMovieList();
      await this.getCinemaList();
      this.formMode = this.$_MISAEnum.formAdd;
      this.isShowForm = true;
    },
    async openFormDetail(index) {
      await this.getMovieList();
      await this.getCinemaList();
      this.formMode = this.$_MISAEnum.formUpdate;
      this.isShowForm = true;
      this.screeningSelected = this.screeningList[index];
    },
    async deleteScreening() {
      let success = false;
      const idsToDelete = this.selectedScreeningIds.map((item) => item.toString());

      // Gửi yêu cầu DELETE với dữ liệu dạng JSON
      await instance
        .delete("Screening", { data: idsToDelete })
        .then(() => {
          success = true;
        })
        .catch((err) => console.log(err));

      if (success) {
        this.getScreeningList();
        this.showToastSuccess("Xóa thành công");
      }
    },
    hideForm() {
      this.isShowForm = false;
      this.movieList = [];
      this.cinemaList = [];
    },
    clickOnCheckbox(index, id) {
      if (this.$refs["checkbox"][index].isChecked) {
        this.selectedScreeningIds = this.selectedScreeningIds.filter(
          (idMovie) => {
            return id !== idMovie;
          }
        );
      } else {
        this.selectedScreeningIds.push(id);
      }
      this.$refs["checkbox"][index].isChecked =
        !this.$refs["checkbox"][index].isChecked;
      if (this.screeningList.length === this.selectedScreeningIds.length) {
        this.$refs["checkbox-all"].isChecked = true;
      } else {
        this.$refs["checkbox-all"].isChecked = false;
      }
    },

    handleScroller() {
      this.$refs.tableHeader.scrollLeft = this.$refs.tableContent.scrollLeft;
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
    formatedDateTime(date) {
      return formatDateTime(date)
    },
    convertTime(originalTimeString) {
      // Bước 1: Chuyển đổi chuỗi thành đối tượng Date
      const dateObj = new Date(originalTimeString);

      // Bước 2: Trích xuất các thành phần ngày, tháng, năm, giờ, phút và giây
      const year = dateObj.getFullYear();
      const month = String(dateObj.getMonth() + 1).padStart(2, "0");
      const day = String(dateObj.getDate()).padStart(2, "0");
      const hours = String(dateObj.getHours()).padStart(2, "0");
      const minutes = String(dateObj.getMinutes()).padStart(2, "0");
      const seconds = String(dateObj.getSeconds()).padStart(2, "0");

      // Bước 3: Tạo chuỗi mới với định dạng "YYYY-MM-DDTHH:mm:ss"
      const formattedString = `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
      return formattedString
    },
  },
};
</script>

<style scoped></style>
