<template>
  <div class="booking-detail">
    <div class="movie-name">{{ this.movieName }}</div>
    <div class="booking-cinema">
      <div
        v-for="cinema in this.cinemaList"
        :key="cinema.cinemaId"
        class="cinema--name"
        :class="{ selected: this.cinemaSelectedId == cinema.cinemaId }"
        @click="handleChooseCinema(cinema.cinemaId, cinema.cinemaName)"
      >
        {{ cinema.cinemaName }}
      </div>
    </div>
    <div class="booking-date">
      <div
        v-for="(data, index) in dateList"
        :key="index"
        class="date"
        @click="this.handleChooseDate(index)"
        :class="{ selected: this.selectedDateIndex === index }"
      >
        <div class="small">
          <span>{{ data.month }}</span>
          <em>{{ data.dayOfWeekName }}</em>
        </div>
        <div class="big">
          <strong>{{ data.date }}</strong>
        </div>
      </div>
    </div>
    <div class="not-found-screening" v-if="!this.screeningList.length">
      Không có suất chiếu nào vào ngày này
    </div>
    <div class="booking-time" v-if="this.screeningList.length">
      <div
        v-for="(val, index) in screeningList"
        :key="val.screenId"
        class="booking-time--scope"
        @click="
          this.handleChooseScreening(val.screeningId, val.screenId, index)
        "
      >
        <div class="booking-time--screen">{{ val.screenName }}</div>
        <div
          class="time"
          :class="{ selected: this.screeningSelectedId === val.screeningId }"
        >
          {{ formatDateToAMPM(val.timeStart) }}
        </div>
      </div>
    </div>
    <div class="booking-seat" v-if="this.seatList.length">
      <img src="../assets/icon/film-image/bg-screen.png" class="screen" />
      <div class="seats">
        <t-seat
          v-for="(seat, index) in this.seatList"
          :key="seat.SeatId"
          :isOccupied="seat.Status"
          :name="seat.Position"
          :type="seat.SeatType"
          @click="this.handleSelectSeat(index)"
        ></t-seat>
      </div>
    </div>
    <div class="notice" v-if="this.seatList.length">
      <div class="notice-list">
        <div class="icon--checked">Checked</div>
        <div class="icon--selected">Đã chọn</div>
      </div>
      <div class="notice-list">
        <div class="icon--normal">Ghế thường</div>
        <div class="icon--vip">Ghế vip</div>
      </div>
    </div>
    <div class="summary" v-if="this.seatList.length">
      <div class="total">
        Tổng: <b>{{ this.totalPrice }}</b> VNĐ
      </div>
      <div class="btn-actions">
        <t-button
          label="Đặt chỗ"
          class="btn--primary"
          @click="handleBooking"
        ></t-button>
        <t-button
          @click="this.handleCheckout"
          label="Thanh toán"
          class="btn--primary"
        ></t-button>
      </div>
    </div>
  </div>
</template>

<script scoped>
import instance from "../common/instance";
export default {
  name: "BookingDetail",
  props: {
    movieName: String,
  },
  data() {
    return {
      cinemaList: [],
      dateList: [],
      screeningList: [],
      cinemaSelectedId: "",
      cinemaSelectedName: "",
      selectedDateIndex: -1,
      screeningSelectedId: "",
      screenSelectedId: "",
      seatList: [],
      totalPrice: 0,
      selectedSeats: [],
      screeningPrice: 0,
      seats: {},
    };
  },
  async created() {
    await this.getCinema();
    this.getFutureDates();
    this.cinemaSelectedName = this.cinemaList[0].cinemaName;
    this.cinemaSelectedId = this.cinemaList[0].cinemaId;
  },
  watch: {
    cinemaSelectedName: async function (newValue) {
      if (this.selectedDateIndex >= 0) {
        await this.getScreeningFilter(
          this.dateList[this.selectedDateIndex],
          newValue
        );
      }
    },
    selectedDateIndex: async function (newValue) {
      if (this.cinemaSelectedName) {
        await this.getScreeningFilter(
          this.dateList[newValue],
          this.cinemaSelectedName
        );
      }
    },
    screeningSelectedId: async function () {
      await this.getSeatFilter();
    },
    selectedSeats: {
      handler: function () {
        this.calculateTotal();
      },
      deep: true,
    },
  },
  methods: {
    async getCinema() {
      await instance
        .get("Cinema")
        .then((res) => {
          this.cinemaList = res.data;
        })
        .catch((err) => console.log(err));
    },
    addDaysToDate(date, days) {
      const result = new Date(date);
      result.setDate(result.getDate() + days);
      return result;
    },

    getDayOfWeekName(dayIndex) {
      const daysOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
      return daysOfWeek[dayIndex];
    },

    getFutureDates() {
      const today = new Date();
      const futureDates = [];

      for (let i = -2; i <= 19; i++) {
        let futureDate = this.addDaysToDate(today, i);
        let day = futureDate.getDay(); // 0: Chủ nhật, 1: Thứ hai, ..., 6: Thứ bảy
        let dayOfWeekName = this.getDayOfWeekName(day);
        let date = futureDate.getDate().toString().padStart(2, "0");
        let month = futureDate.getMonth() + 1; // Lưu ý rằng tháng bắt đầu từ 0 (tháng 0 là tháng 1)
        let year = futureDate.getFullYear();

        futureDates.push({ dayOfWeekName, date, month, year });
      }

      this.dateList = futureDates;
    },

    formatedDate(date) {
      return `${date.year}-${date.month}-${date.date}`;
    },
    formatDateToAMPM(dateTimeString) {
      const date = new Date(dateTimeString);
      const hours = date.getHours();
      const minutes = date.getMinutes();

      // Chuyển đổi giờ từ 24 giờ sang 12 giờ (AM/PM)
      const ampm = hours >= 12 ? "PM" : "AM";
      const formattedHours = hours % 12 || 12;

      // Đảm bảo định dạng hh:mm AM/PM
      const formattedTime = `${formattedHours
        .toString()
        .padStart(2, "0")}:${minutes.toString().padStart(2, "0")} ${ampm}`;

      return formattedTime;
    },
    handleChooseCinema(id, name) {
      this.cinemaSelectedId = id;
      this.cinemaSelectedName = name;
    },
    handleChooseDate(index) {
      this.selectedDateIndex = index;
      this.seatList = [];
      this.screeningSelectedId = "";
      this.totalPrice = 0;
      this.selectedSeats = [];
    },
    handleChooseScreening(idScreening, idScreen, index) {
      this.screeningSelectedId = idScreening;
      this.screenSelectedId = idScreen;
      console.log(this.screeningList);
      this.screeningPrice = this.screeningList[index].ticketPrice;
    },
    async getScreeningFilter(date, cinemaName) {
      date = this.formatedDate(date);
      await instance
        .get(`Screening/Filter?date=${date}&cinemaName=${cinemaName}`)
        .then((res) => {
          console.log(res.data);
          this.screeningList = res.data;
        })
        .catch((err) => console.log(err));
    },
    async getSeatFilter() {
      await instance
        .get(
          `Seat/Filter?screenId=${this.screenSelectedId}&screeningId=${this.screeningSelectedId}`
        )
        .then((res) => {
          this.seatList = res.data;
        })
        .catch((err) => console.log(err));
    },
    handleSelectSeat(index) {
      if (this.selectedSeats.includes(this.seatList[index])) {
        delete this.seats[this.seatList[index].SeatId];

        this.selectedSeats = this.selectedSeats.filter((seat) => {
          return seat.SeatId !== this.seatList[index].SeatId;
        });
      } else {
        let price = 0;
        if (this.seatList[index].SeatType == 0) {
          price = this.screeningPrice;
        } else {
          price = this.screeningPrice * 1.05;
        }
        this.seats[this.seatList[index].SeatId] = price;
        this.selectedSeats.push(this.seatList[index]);
      }
    },
    calculateTotal() {
      this.totalPrice = this.selectedSeats.reduce((total, element) => {
        switch (element.SeatType) {
          case 0:
            return total + this.screeningPrice;
          case 1:
            return total + this.screeningPrice * 1.05;
          default:
            break;
        }
      }, 0);
    },
    handleCheckout() {
      if (this.selectedSeats.length == 0) {
        alert("Vui lòng chọn vị trí ngồi!")
      }
      else if (!sessionStorage.getItem("jwtToken")) {
        this.$router.push("/login");
      } else {
        const bookingData = {
          ScreeningId: this.screeningSelectedId,
          userId: sessionStorage.getItem("jwtToken")
            ? JSON.parse(atob(sessionStorage.getItem("jwtToken").split(".")[1]))
                .UserId
            : null,
          BookingDate: new Date(new Date().getTime() + 7 * 60 * 60000),
          Amount: this.totalPrice,
          // PaymentMethod: "Credit Card",
          PaymentStatus: 1,
        };
        instance
          .post("Booking", {
            booking: bookingData,
            seats: this.seats,
          })
          .then(() => {
            this.$emit("closeModal");
            this.$emit("showToastSuccess", "Mua vé thành công");
          })
          .catch((err) => console.log(err));
      }
    },
    handleBooking() {
      if (this.selectedSeats.length == 0) {
        alert("Vui lòng chọn vị trí ngồi!")
      }
      else if (!sessionStorage.getItem("jwtToken")) {
        this.$router.push("/login");
      } else {
        const bookingData = {
          ScreeningId: this.screeningSelectedId,
          userId: sessionStorage.getItem("jwtToken")
            ? JSON.parse(atob(sessionStorage.getItem("jwtToken").split(".")[1]))
                .UserId
            : null,
          BookingDate: new Date(new Date().getTime() + 7 * 60 * 60000),
          Amount: this.totalPrice,
          PaymentStatus: 0,
        };
        instance
          .post("Booking", {
            booking: bookingData,
            seats: this.seats,
          })
          .then(() => {
            this.$emit("closeModal");
            this.$emit("showToastSuccess", "Đặt vé thành công");
          })
          .catch((err) => console.log(err));
      }
    },
  },
};
</script>

<style scoped>
@import url(../css/views/bookingdetail.css);
@import url(../css/components/icon.css);
</style>
