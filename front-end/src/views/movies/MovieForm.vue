<template>
  <m-modal>
    <div class="popup">
      <div class="popup__header">
        <div class="popup__header__title">{{ this.titleForm }}</div>
        <div
          id="btnCloseModalAdd"
          class="popup__header__close"
          @click="this.$emit('hideAddForm')"
        >
          <div class="icon--close"></div>
        </div>
      </div>
      <div class="popup__container">
        <div class="popup__row">
          <div class="popup__row-col-6">
            <div>Tên phim</div>
            <m-text-input v-model="this.movie.movieTitle"></m-text-input>
          </div>
          <div class="popup__row-col-6">
            <div>Đạo diễn</div>
            <m-text-input v-model="this.movie.movieDirector"></m-text-input>
          </div>
        </div>
        <div class="popup__row">
          <div class="popup__row-col-6">
            <div>Diễn viên</div>
            <m-text-input v-model="this.movie.movieActors"></m-text-input>
          </div>
          <div class="popup__row-col-6">
            <div>Thể loại</div>
            <m-combo-box
              placeholder=" "
              v-model="this.movie.genreName"
              :dataSelect="this.genreObject"
            ></m-combo-box>
          </div>
        </div>
        <div class="popup__row">
          <div class="popup__row-col-6">
            <div>Ngày khởi chiếu</div>
            <m-date-picker v-model="this.movie.yearOfRelease"></m-date-picker>
          </div>
          <div class="popup__row-col-6">
            <div>Thời lượng (phút)</div>
            <m-text-input v-model="this.movie.duration"></m-text-input>
          </div>
        </div>
        <div class="popup__row">
          <div class="popup__row-col-6">
            <div>Ngôn ngữ</div>
            <m-text-input v-model="this.movie.language"></m-text-input>
          </div>
          <div class="popup__row-col-6">
            <div>Ghi chú</div>
            <m-text-input></m-text-input>
          </div>
        </div>
        <div class="popup__row">
          <div class="popup__row-col-3">
            <div>Hình ảnh phim</div>
            <div class="image__preview">
              <img :src="this.movie.image" alt="Not found" />
            </div>
            <input type="file" @change="onImageChange" accept="image/*" />
          </div>
          <div class="popup__row-col-9">
            <div>Tóm tắt nội dung</div>
            <!-- <m-text-input
              individualClass="text__input--large"
              v-model="this.movie.summary"
            ></m-text-input> -->
            <textarea class="text-area" rows="4" cols="50" placeholder="Nhập dữ liệu vào đây..." v-model="this.movie.summary"></textarea>
          </div>
        </div>
      </div>
      <div class="popup__action-btn">
        <m-button
          individualClass="btn--primary"
          label="Lưu"
          @click="this.actionMovie"
        ></m-button>
        <m-button
          individualClass="btn--noborder"
          label="Hủy"
          @click="this.$emit('hideAddForm')"
        ></m-button>
      </div>
    </div>
  </m-modal>
</template>

<script>
export default {
  name: "MovieForm",
  props: {
    movieSelected: Object,
    formMode: Number,
    genreObject: Array,
  },
  data() {
    return {
      previewImage: null,
      titleForm: "Thêm phim mới",
      movie: {},
    };
  },
  created() {
    switch (this.formMode) {
      case this.$_MISAEnum.formAdd:
        this.titleForm = "Thêm phim mới";
        break;
      case this.$_MISAEnum.formUpdate:
        this.titleForm = "Sửa phim";
        this.movie = { ...this.movieSelected };
        break;
      default:
        break;
    }
  },
  watch: {
    "movie.genreName": function (newValue) {
      const option = this.genreObject.find((element) => {
        return element.label == newValue;
      });
      if (option) {
        this.movie.genreId = option.id;
      }
    },
  },
  methods: {
    onImageChange(event) {
      const file = event.target.files[0];
      if (file) {
        const reader = new FileReader();
        reader.onloadend = () => {
          this.movie.image = reader.result;
        };
        reader.readAsDataURL(file);
      } else {
        this.movie.image = null;
      }
    },
    actionMovie() {
      switch (this.formMode) {
        case this.$_MISAEnum.formAdd:
          this.$emit("insertMovie", this.movie);
          break;
        case this.$_MISAEnum.formUpdate:
          this.$emit("updateMovie", this.movie);
          break;
        default:
          break;
      }
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
  border-color:  #afafaf;
  padding: 12px;
}
</style>
