<template>
  <InfoSidebar></InfoSidebar>
  <InfoContent></InfoContent>
</template>

<script scoped>
import InfoContent from "./infodetail/InfoContent.vue";
import InfoSidebar from "./infodetail/InfoSidebar.vue";
export default {
  name: "InfoDetail",
  components: {
    InfoContent,
    InfoSidebar,
  },
  data() {},
  methods: {},
};
</script>

<style scoped>
@import url(../css/views/infodetail.css);
</style>
