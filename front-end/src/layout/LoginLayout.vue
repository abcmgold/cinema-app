<template>
    <div class="login">
        <div class="login-form">
            <div class="login-form__title">Đăng Nhập</div>
            <div class="login-form__input">
                <div class="login-form__input--group">
                    <div class="login--group__title">Tên đăng nhập</div>
                    <div class="login--group__input" @keydown.enter="login" >
                        <m-text-input  v-model="this.username"></m-text-input>
                    </div>
                </div>
                <div class="login-form__input--group">
                    <div class="login--group__title">Mật khẩu</div>
                    <div class="login--group__input" @keydown.enter="login">
                        <m-text-input type="password" v-model="this.password"></m-text-input>
                    </div>
                </div>
                <div class="login--message" v-show="this.showMessageValidate">{{this.errorMessage}}</div>
            </div>
            <div class="login-form__btn">
                <m-button class="btn btn--primary" label="Đăng nhập" @click="this.login"></m-button>
                <a class="forgot--link" href="#">Quên mật khẩu?</a>
                <router-link to="signup" class="forgot--link" href="#">Đăng ký</router-link>
            </div>
        </div>
    </div>
</template>
<script scoped>
import instance from '../common/instance'
export default {
  name: "LoginLayout",
  components: {},
  data() {
    return {
        username: '',
        password: '',
        showMessageValidate: false,
        errorMessage: ''
    }
  },

  watch: {
    username: function() {
        this.showMessageValidate = false;
    },
    password: function() {
        this.showMessageValidate = false;
    },
  },
  methods: {
    async login() {
        if (this.username.trim() && this.password.trim()) {
            await instance.post(`Account/Login?username=${this.username}&password=${this.password}`)
            .then((res) =>  {
                sessionStorage.setItem("jwtToken", res.data.jwtToken);
                sessionStorage.setItem("username", res.data.username);
                this.$store.commit('login')
                this.$router.push('/')
            })
            .catch((err) =>{
                this.showMessageValidate = true;
                this.errorMessage = err.response.data
            })
        }
        else {
            this.showMessageValidate = true;
            this.errorMessage = "Vui lòng nhập đầy đủ cả Email và Password"
        }
       
    }
  }
};
</script>
<style>
@import url(../css/layout/login.css);
</style>
