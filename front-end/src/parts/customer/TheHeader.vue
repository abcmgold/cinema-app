<template>
  <div class="header">
    <router-link to="/">
      <div class="header__logo">Movie88</div>
    </router-link>
    <div class="header__options">
      <ul class="header--items">
        <li class="header--item">Phim</li>
        <router-link to="/customer/ticket">
          <li class="header--item">Lịch sử giao dịch</li>
        </router-link>
        <li class="header--item">Ưu đãi</li>
        <li class="header--item">Tuyển dụng</li>
      </ul>
    </div>
    <div class="header__actions">
      <div v-if="!this.jwtToken" class="header__actions--unlogin">
        <router-link to="/signup">
          <m-button label="Đăng ký" individualClass="btn--primary"></m-button>
        </router-link>
        <router-link to="/login">
          <m-button
            label="Đăng nhập"
            individualClass="btn--primary--red"
          ></m-button>
        </router-link>
      </div>
      <div v-if="this.jwtToken" class="header__actions--login">
        <div class="icon--user"></div>
        <div @click="this.showMenu" class="logout-msg">
          XIN CHÀO, {{ this.username }}!
          <ul v-if="this.isShowMenu" class="function-list">
            <router-link to="/customer">
              <li>Thông tin cá nhân</li>
            </router-link>
            <router-link to="/admin/cinema" v-if="userRole === 'Admin'">
              <li>Quản lý rạp</li> </router-link
            ><router-link to="/admin/movie" v-if="userRole === 'Admin'">
              <li>Quản lý phim</li> </router-link
            ><router-link to="/admin/ticket" v-if="userRole === 'Admin'">
              <li>Quản lý vé</li> </router-link
            ><router-link to="/admin/screening" v-if="userRole === 'Admin'">
              <li>Quản lý suất chiếu</li> </router-link
            ><router-link to="/admin/screen" v-if="userRole === 'Admin'">
              <li>Quản lý phòng chiếu</li> </router-link
            ><router-link to="/admin/employee" v-if="userRole === 'Admin'">
              <li>Quản lý nhân viên</li>
            </router-link>
          </ul>
        </div>
        <div @click="this.logout" class="logout-btn">THOÁT</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "TheHeader",
  data() {
    return {
      jwtToken: "",
      isShowMenu: false,
      username: "",
      userRole: "",
    };
  },
  created() {
    this.jwtToken = sessionStorage.getItem("jwtToken");
    this.username = this.toUpperCase(sessionStorage.getItem("username"));

    this.userRole = sessionStorage.getItem("jwtToken")
      ? JSON.parse(atob(sessionStorage.getItem("jwtToken").split(".")[1])).role
      : null;
  },

  methods: {
    logout() {
      sessionStorage.removeItem("jwtToken");
      this.$router.push("/login");
    },
    showMenu() {
      this.isShowMenu = !this.isShowMenu;
    },
    toUpperCase(value) {
      if (value == null) {
        return "";
      } else return value.toUpperCase();
    },
  },
};
</script>

<style scoped>
@import url(../../css/layout/header.css);
@import url(../../css/components/icon.css);
</style>
