<template>
  <div class="movie__detail">
    <div class="title">NỘI DUNG PHIM</div>
    <div class="movie__description">
      <div class="movie__thumb">
        <img :src="this.movieDetail.image" class="movie__image" />
        <t-button
          label="Mua vé"
          class="btn--book"
          @click="this.showModal"
        ></t-button>
      </div>
      <div class="movie__text">
        <div class="movie__name">{{this.movieDetail.movieTitle}}</div>
        <div class="movie__info">
          <ul class="movie__info--list">
            <li class="movie__info--item">
              <strong>Đạo diễn: </strong>{{this.movieDetail.movieDirector}}
            </li>
            <li class="movie__info--item">
              <strong>Diễn viên: </strong>{{this.movieDetail.movieActors}}
            </li>
            <li class="movie__info--item">
              <strong>Thể loại: </strong>{{this.movieDetail.genreName}}
            </li>
            <li class="movie__info--item">
              <strong>Khởi chiếu: </strong>{{convertDate(this.movieDetail.yearOfRelease)}}
            </li>
            <li class="movie__info--item">
              <strong>Thời lượng: </strong>{{this.movieDetail.duration}} phút
            </li>
            <li class="movie__info--item">
              <strong>Ngôn ngữ: </strong>{{this.movieDetail.language}}
            </li>
          </ul>
        </div>
      </div>
    </div>
    <div class="movie__summary">
      <div class="movie__summary--title">Tóm tắt</div>
      <div class="movie__summary--description">{{ this.movieDetail.summary }}
      </div>
    </div>
  </div>
  <t-modal v-if="this.isShowModal" @closeModal="closeModal">
    <BookingDetail></BookingDetail>
  </t-modal>
</template>

<script scoped>
import BookingDetail from "../views/BookingDetail.vue";
import instance from "../common/instance";
import dayjs from 'dayjs';
import { delay } from '@/common/common';
export default {
  name: "MovieDetail",
  components: {
    BookingDetail,
  },
  async created() {
    await this.getMovieDetail();
  },
  data() {
    return {
      isShowModal: false,
      movieDetail: {},
      isShowToastSuccess: false,
      isShowToastError: false,
      labelToastSuccess: "",
      labelToastError: "",
    };
  },
  methods: {
    closeModal() {
      this.isShowModal = false;
    },
    showModal() {
      this.isShowModal = true;
    },
    async getMovieDetail() {
      let id = this.$route.params.movieId;
      this.$store.commit('showMask')
      await instance
        .get(`Movie/${id}`)
        .then(async (result) => {
          await delay(500)
          this.$store.commit('hideMask')
          this.movieDetail = result.data
        })
        .catch((error) => console.log(error));
    },
    convertDate(dateString) {
      // Phân tích chuỗi thành đối tượng ngày
      const dateObject = dayjs(dateString);

      // Chuyển đổi về định dạng chuỗi mong muốn
      return dateObject.format("DD-MM-YYYY");
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

<style scoped>
@import url(../css/views/moviedetail.css);
</style>
