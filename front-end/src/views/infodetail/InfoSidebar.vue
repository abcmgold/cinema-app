<template>
  <div class="info__sidebar">
    <div class="info__sidebar--title">TÀI KHOẢN MOVIE88</div>
    <ul class="info__items">
      <router-link to="/customer/general" class="info__items--title">THÔNG TIN CHUNG</router-link>
      <router-link to="/customer/ticket" class="info__items--title">LỊCH SỬ GIAO DỊCH</router-link>
    </ul>
  </div>
</template>

<script scoped>
export default {
  name: "InfoSidebar",
  components: {},
};
</script>

<style scoped>
@import url(../../css/views/infosidebar.css);
</style>
