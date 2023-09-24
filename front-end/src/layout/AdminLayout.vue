<template>
  <div class="container">
    <AdminSidebar></AdminSidebar>
    <div class="main">
      <AdminHeader></AdminHeader>
      <AdminContent></AdminContent>
    </div>
  </div>
</template>
<script scoped>
import AdminHeader from "@/parts/admin/AdminHeader.vue";
import AdminSidebar from "@/parts/admin/AdminSidebar.vue";
import AdminContent from "@/parts/admin/AdminContent.vue";
export default {
  name: "AdminLayout",
  components: {
    AdminHeader,
    AdminSidebar,
    AdminContent
  },
};
</script>
<style scoped>
.container {
  display: flex;
  flex-direction: row;
}
.main {
  display: flex;
  flex-direction: column;
  flex: 1;
  width: 600px;
}
</style>
