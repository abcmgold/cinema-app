<template>
  <div class="signup">
    <div class="signup-form">
      <div class="signup-form__title">Đăng ký</div>
      <div class="signup-form__input">
        <div class="signup-form__input--group">
          <div class="signup--group__title">
            Họ tên<span style="color: red">*</span>
          </div>
          <div class="signup--group__input" @keydown.enter="signup">
            <m-text-input v-model="this.fullname"></m-text-input>
          </div>
        </div>
        <div class="signup-form__input--group">
          <div class="signup--group__title">
            Số điện thoại<span style="color: red">*</span>
          </div>
          <div class="signup--group__input" @keydown.enter="signup">
            <m-text-input v-model="this.phoneNumber"></m-text-input>
          </div>
        </div>
        <div class="signup-form__input--group">
          <div class="signup--group__title">
            Email<span style="color: red">*</span>
          </div>
          <div class="signup--group__input" @keydown.enter="signup">
            <m-text-input v-model="this.email"></m-text-input>
          </div>
        </div>
        <div class="signup-form__input--group">
          <div class="signup--group__title">
            Tên đăng nhập<span style="color: red">*</span>
          </div>
          <div class="signup--group__input" @keydown.enter="signup">
            <m-text-input v-model="this.username"></m-text-input>
          </div>
        </div>
        <div class="signup-form__input--group">
          <div class="signup--group__title">
            Mật khẩu<span style="color: red">*</span>
          </div>
          <div class="signup--group__input" @keydown.enter="signup">
            <m-text-input
              type="password"
              v-model="this.password"
            ></m-text-input>
          </div>
        </div>
        <div class="signup-form__input--group">
          <div class="signup--group__title">
            Nhập lại mật khẩu<span style="color: red">*</span>
          </div>
          <div class="signup--group__input" @keydown.enter="signup">
            <m-text-input
              type="password"
              v-model="this.confirmPassword"
            ></m-text-input>
          </div>
        </div>
        <div class="signup--message" v-show="this.showMessageValidate">
          {{ this.errorMessage }}
        </div>
      </div>
      <div class="signup-form__btn">
        <m-button
          class="btn btn--primary"
          label="Đăng ký"
          @click="this.signup"
        ></m-button>
        <span>Đã có tài khoản?</span>
        <router-link to="signup" class="forgot--link" href="#"
          >Đăng nhập</router-link
        >
      </div>
    </div>
  </div>
</template>
<script scoped>
import instance from "../common/instance";
export default {
  name: "signupLayout",
  components: {},
  data() {
    return {
      username: "",
      password: "",
      fullname: "",
      phoneNumber: "",
      email: "",
      confirmPassword: "",
      showMessageValidate: false,
      errorMessage: "",
    };
  },

  watch: {
    username: function () {
      this.showMessageValidate = false;
    },
    password: function () {
      this.showMessageValidate = false;
    },
  },
  methods: {
    async signup() {
      if (
        this.username.trim() &&
        this.password.trim() &&
        this.fullname.trim() &&
        this.phoneNumber.trim() &&
        this.email.trim() &&
        this.confirmPassword == this.password
      ) {
        await instance
          .post("Account/Signup", {
            UserName: this.username,
            Password: this.password,
            FullName: this.fullname,
            Email: this.email,
            PhoneNumber: this.phoneNumber
          })
          .then(() => {
            alert("Đăng ký thành công")
            this.$router.push("/login")
          })
          .catch((err) => {
            this.showMessageValidate = true;
            this.errorMessage = err.response.data;
          });
      } else if (
        !this.username.trim() ||
        !this.password.trim() ||
        !this.fullname.trim() ||
        !this.phoneNumber.trim() ||
        !this.email.trim()
      ) {
        this.showMessageValidate = true;
        this.errorMessage = "Vui lòng nhập đầy đủ thông tin";
      } else if (this.confirmPassword != this.password) {
        console.log(this.confirmPassword, this.password);
        this.showMessageValidate = true;
        this.errorMessage = "Xác nhận mật khẩu không trùng khớp mật khẩu!";
      }
    },
  },
};
</script>
<style>
@import url(../css/layout/signup.css);
</style>
