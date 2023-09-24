<template>
  <m-modal>
    <div class="popup">
      <div class="popup__header">
        <div class="popup__header__title">{{ this.titleForm }}</div>
        <div
          id="btnCloseModalAdd"
          class="popup__header__close"
          @click="this.$emit('hideForm')"
        >
          <div class="icon--close"></div>
        </div>
      </div>
      <div class="popup__container">
        <div class="popup__row">
          <div class="popup__row-col-6">
            <div>Tên phim</div>
            <m-combo-box
              placeholder=" "
              v-model="this.screening.movieTitle"
              :dataSelect="this.movieList"
            ></m-combo-box>
          </div>
          <div class="popup__row-col-6">
            <div>Rạp chiếu</div>
            <m-combo-box
              placeholder=" "
              v-model="this.screening.cinemaName"
              :dataSelect="this.cinemaList"
            ></m-combo-box>
          </div>
        </div>
        <div class="popup__row">
          <div class="popup__row-col-6">
            <div>Phòng chiếu</div>
            <m-combo-box
              placeholder=" "
              v-model="this.screening.screenName"
              :dataSelect="this.screenList"
            ></m-combo-box>
          </div>
          <div class="popup__row-col-6">
            <div>Giá vé</div>
            <m-text-input v-model="this.screening.ticketPrice"></m-text-input>
          </div>
        </div>
        <div class="popup__row">
          <div class="popup__row-col-6">
            <div>Thời gian bắt đầu</div>
            <m-date-picker v-model="this.screening.timeStart"></m-date-picker>
          </div>
          <div class="popup__row-col-6">
            <div>Thời gian kết thúc</div>
            <m-date-picker v-model="this.screening.timeEnd"></m-date-picker>
          </div>
        </div>
      </div>
      <div class="popup__action-btn">
        <m-button
          individualClass="btn--primary"
          label="Lưu"
          @click="this.actionScreening"
        ></m-button>
        <m-button
          individualClass="btn--noborder"
          label="Hủy"
          @click="this.$emit('hideForm')"
        ></m-button>
      </div>
    </div>
  </m-modal>
</template>

<script>
import instance from "../../common/instance";
export default {
  name: "ScreeningForm",
  props: {
    screeningSelected: Object,
    formMode: Number,
    cinemaList: Array,
    movieList: Array,
  },
  data() {
    return {
      previewImage: null,
      titleForm: "Thêm suất chiếu mới",
      screening: {},
      screenList: [],
    };
  },
  async created() {
    switch (this.formMode) {
      case this.$_MISAEnum.formAdd:
        this.titleForm = "Thêm suất chiếu mới";
        break;
      case this.$_MISAEnum.formUpdate:
        this.titleForm = "Sửa suất chiếu";
        this.screening = { ...this.screeningSelected };
        break;
      default:
        break;
    }
    if (this.screening.cinemaId) {
      this.getScreenList(this.screening.cinemaId);
    }
  },
  watch: {
    "screening.cinemaName": async function (newValue) {
      this.screenList  = []
      let id = this.cinemaList.find((value) => {
        return value.value == newValue;
      })
      if (id) {
        this.screening.cinemaId = id.id;
        await this.getScreenList(this.screening.cinemaId);
      }
    },
    "screening.movieTitle": async function (newValue) {
      console.log(this.movieList);
      let id = this.movieList.find((value) => {
        return value.value == newValue;
      }).id;
      console.log(id);
      this.screening.movieId = id;
    },
    "screening.screenName": async function (newValue) {
      if (this.screenList.length) {
        let id = this.screenList.find((value) => {
          return value.value == newValue;
        });
        if (id) {
          id = id.id;
        }
        this.screening.screenId = id;
      }
    },
  },
  methods: {
    actionScreening() {
      switch (this.formMode) {
        case this.$_MISAEnum.formAdd:
          this.$emit("insertScreening", this.screening);
          break;
        case this.$_MISAEnum.formUpdate:
          console.log(this.screening);
          this.$emit("updateScreening", this.screening);
          break;
        default:
          break;
      }
    },
    async getScreenList(id) {
      await instance
        .get(`Screen/FilterByName?name=${id}`)
        .then((res) => {
          res.data.forEach((value) => {
            this.screenList.push({
              id: value.screenId,
              value: value.screenName,
              label: value.screenName,
            });
          });
        })
        .catch((err) => console.log(err));
    },
    
  },
};
</script>

<style scoped>
@import url(../../css/components/popup.css);
.text-area {
  width: 100%;
  height: 200px;
  outline: none;
  border-color: #afafaf;
  padding: 12px;
}
</style>
