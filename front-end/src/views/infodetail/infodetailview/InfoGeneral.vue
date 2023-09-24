<template>
  <div class="info__general">
    <div class="info__general__title">THÔNG TIN TÀI KHOẢN</div>
    <div class="info__general__content">
      <ul class="info__general__list">
        <li>Họ và tên: {{ this.info.fullname }}</li>
        <li>Email: {{ this.info.email }}</li>
        <li>Điện thoại: {{ this.info.phoneNumber }}</li>
      </ul>
      <div class="info__general--avatar">
        <div class="info__general--image">
          <img
            v-if="!this.info.image"
            src="../../../assets/icon/avatar-default.png"
          />
          <img v-if="this.info.image" :src="this.info.image" />
        </div>
        <input
          type="file"
          ref="fileInput"
          style="display: none"
          @change="handleFileChange"
        />
        <button @click="openFileInput">Thay đổi</button>
      </div>
    </div>
  </div>
  <m-toast
    :label="this.labelToastSuccess"
    icon="icon--success"
    v-if="isShowToastSuccess"
  ></m-toast>
  <m-toast
    :label="this.labelToastError"
    icon="icon--error"
    v-if="isShowToastError"
  ></m-toast>
</template>

<script scoped>
import instance from "../../../common/instance";
export default {
  name: "InfoGeneral",
  components: {},
  data() {
    return {
      info: {},
      isShowToastSuccess: false,
      isShowToastError: false,
      labelToastSuccess: "",
      labelToastError: "",
    };
  },
  created() {
    let accountId = sessionStorage.getItem("jwtToken")
      ? JSON.parse(atob(sessionStorage.getItem("jwtToken").split(".")[1]))
          .AccountId
      : null;
    console.log(accountId);
    instance
      .get(`Account/GetInfo?accountId=${accountId}`)
      .then((res) => {
        this.info.userId = res.data.userId;
        this.info.email = res.data.email;
        this.info.fullname = res.data.fullName;
        this.info.phoneNumber = res.data.phoneNumber;
        this.info.username = res.data.userName;
        this.info.image = res.data.image;
        this.info.accountId = accountId;
      })
      .catch((err) => {
        console.log(err);
      });
  },

  watch: {
    "this.info.image": async function () {
      console.log(1234);
      await this.updateInfo();
    },
  },

  methods: {
    openFileInput() {
      // Khi nút được nhấp, kích hoạt sự kiện click trên thẻ input file ẩn
      this.$refs.fileInput.click();
    },
    async handleFileChange(event) {
      console.log(12);
      // Xử lý tập tin sau khi người dùng đã chọn tập tin từ máy tính
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();

        const readFile = () => {
          return new Promise((resolve, reject) => {
            reader.onloadend = () => {
              resolve(reader.result);
            };
            reader.onerror = reject;
            reader.readAsDataURL(file);
          });
        };
        try {
          this.info.image = await readFile();
        } catch (error) {
          console.error("Error reading file:", error);
        }
      } else {
        this.info.image = "";
      }
      await this.updateInfo();
    },

    async updateInfo() {
      await instance
        .put(`User/${this.info.userId}`, this.info)
        .then(() => {
          this.showToastSuccess('Thay đổi thành công')
        })
        .catch(() => {
          this.showToastError('Đã có lỗi xảy ra, vui lòng thử lại!')
        });
    },
    showToastSuccess(label) {
      this.isShowToastSuccess = true;
      this.labelToastSuccess = label;
      setTimeout(() => {
        this.isShowToastSuccess = false;
      }, 3000);
    },
    showToastError(label) {
      this.isShowToastError = true;
      this.labelToastError = label;
      setTimeout(() => {
        this.isShowToastError = false;
      }, 3000);
    },
  },
};
</script>

<style scoped>
@import url(../../../css/views/infodetail.css);
</style>
