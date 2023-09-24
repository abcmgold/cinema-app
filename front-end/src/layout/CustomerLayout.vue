<template>
  <TheHeader></TheHeader>
  <TheContent></TheContent>
  <TheFooter style="display: none"></TheFooter>
</template>
<script scoped>
import TheContent from "../parts/customer/TheContent.vue";
import TheHeader from "../parts/customer/TheHeader.vue";
import TheFooter from "../parts/customer/TheFooter.vue";

export default {
  name: "CustomerLayout",
  components: {
    TheHeader,
    TheContent,
    TheFooter,
  },
};
</script>
<style>
</style>
