<template>
  <div class="card" @mouseenter="hoverEvent" @mouseleave="unHoverEvent">
    <div class="card__item">
      <img class="card__image" :src="this.imageLink" />
      <div class="card__title">{{this.movieTitle}}</div>
      <div class="card__more">
        <div class="card__time">{{this.movieDuration}}phút</div>
        <div class="separate"></div>
        <div class="date">{{convertDate(this.movieDate)}}</div>
      </div>

      <div class="card__modal" v-if="showModalFilm">
        <router-link :to="`/movies/${movieId}`"
          ><m-button label="Chi tiết" individualClass="btn--primary--red"></m-button
        ></router-link>

        <m-button label="Đặt vé" individualClass="btn--primary--red" @click="this.$emit('showModal')"></m-button>
      </div>
    </div>
  </div>
</template>

<script scoped>
import dayjs from 'dayjs';
export default {
  name: "TCardFilm",
  props: {
    imageLink: String,
    movieTitle: String,
    movieDuration: String,
    movieDate: String,
    movieId: String
  },
  data() {
    return {
      showModalFilm: false,
    };
  },
  methods: {
    hoverEvent: function () {
      this.showModalFilm = true;
    },
    unHoverEvent: function () {
      this.showModalFilm = false;
    },
    convertDate(dateString) {
      // Phân tích chuỗi thành đối tượng ngày
      const dateObject = dayjs(dateString);

      // Chuyển đổi về định dạng chuỗi mong muốn
      return dateObject.format("DD-MM-YYYY");
    },
  },
};
</script>

<style scoped>
@import url(../css/components/card.css);
</style>
