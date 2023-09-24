<template>
  <div class="info__content">
    <router-view></router-view>
  </div>
</template>

<script scoped>
export default {
  name: "InfoSidebar",
  components: {},
};
</script>

<style scoped>
.info__content {
  flex: 1;
  padding-right: 120px;
}
</style>
