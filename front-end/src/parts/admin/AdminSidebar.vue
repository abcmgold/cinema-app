<template>
  <div class="sidebar" :class="{ 'sidebar--small': this.isSmallSidebar }">
    <div class="sidebar__logo">
      <div class="sidebar__logo--img"></div>
      <router-link to="/">
        <div class="sidebar__logo--text">Movie 88</div>
      </router-link>
    </div>
    <div class="sidebar__items">
      <MISASidebarItem
        router="/admin/cinema"
        icon="icon--car"
        label="Quản lý rạp"
        arrow="icon--down--big"
      ></MISASidebarItem>

      <MISASidebarItem
        router="/admin/movie"
        icon="icon--home"
        arrow="icon--down--big"
        label="Quản lý phim"
      ></MISASidebarItem>

      <MISASidebarItem
        router="/admin/screening"
        icon="icon--estate"
        arrow="icon--down--big"
        label="Quản lý suất chiếu"
      ></MISASidebarItem>

      <MISASidebarItem
        router="/admin/screen"
        icon="icon--tool"
        arrow="icon--down--big"
        label="Quản lý phòng chiếu"
      ></MISASidebarItem>
      <MISASidebarItem
        router="/admin/ticket"
        icon="icon--tool"
        arrow="icon--down--big"
        label="Quản lý vé"
      ></MISASidebarItem>

      <MISASidebarItem
        router="/admin/employee"
        icon="icon--find"
        arrow="icon--down--big"
        label="Quản lý nhân viên"
      ></MISASidebarItem>
    </div>
    <div class="sidebar__footer">
      <div class="sidebar__footer__icon--left" @click="setSmallSidebar"></div>
      <div class="sidebar__footer__icon--right" @click="setBigSidebar"></div>
    </div>
  </div>
</template>

<script scoped>
import MISASidebarItem from "@/components/MISASidebarItem.vue";
import { MISAResource } from "../../common/resource";
export default {
  components: { MISASidebarItem },
  name: "AdminSidebar",
  data() {
    return {
      isSmallSidebar: false,
      resource: MISAResource,
    };
  },
  methods: {
    setSmallSidebar() {
      this.isSmallSidebar = true;
    },
    setBigSidebar() {
      this.isSmallSidebar = false;
    },
  },
};
</script>

<style scoped>
@import url(@/css/parts/admin/admin-sidebar.css);
</style>
