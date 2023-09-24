<template>
  <div class="container">
    <div v-if="this.$store.getters.isShowMask" class="mask-element"></div>
    <router-view></router-view>
  </div>
</template>
<script scoped>

export default {
  name: "App",
  components: {
  },
};
</script>
<style>
@import url(./css/main.css);
</style>
