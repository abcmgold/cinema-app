<template>
  <div class="movie-list__navbar">
    <div class="movie-list__navbar--left">
      <m-text-input
        v-model="this.filterBookingIdInput"
        individualClass="text__input-icon-start"
        placeholder="Tìm kiếm"
      ></m-text-input>
    </div>
    <div class="movie-list__navbar--right">
      <!-- <m-button
          individualClass="btn--primary"
          label="+ Thêm vé mới"
          @click="this.showForm"
        ></m-button> -->
      <m-button
        @click="this.deleteScreening"
        individualClass="btn--single"
        icon="icon--delete"
      ></m-button>
    </div>
  </div>
  <div class="table">
    <div ref="tableHeader" class="movie-list__table--header">
      <table>
        <thead>
          <tr>
            <th class="align-center" style="width: 50px">
              <div class="table-checkbox">
                <m-checkbox ref="checkbox-all" type="primary"></m-checkbox>
              </div>
            </th>
            <th class="align-center" style="width: 50px">STT</th>
            <th class="align-center" style="width: 200px">Mã vé</th>
            <th class="align-center" style="width: 200px">Người đặt</th>
            <th class="align-center" style="width: 200px">Ngày đặt</th>
            <th class="align-center" style="width: 400px">Tên phim</th>
            <th class="align-center" style="width: 100px">Vị trí ngồi</th>
            <th class="align-center" style="width: 200px">Giá vé</th>
            <th style="width: 227px">Trạng thái</th>
          </tr>
        </thead>
      </table>
    </div>
    <div ref="tableContent" class="movie-list__table" @scroll="handleScroller">
      <table>
        <tbody>
          <tr v-for="(ticket, index) in ticketList" :key="ticket.ticketId">
            <td class="align-center" style="width: 50px">
              <div class="table-checkbox">
                <m-checkbox
                  @click="clickOnCheckbox(index, ticket.ticketId)"
                  ref="checkbox"
                  type="primary "
                ></m-checkbox>
              </div>
            </td>
            <td class="align-center" style="width: 50px">{{ index + 1 }}</td>
            <td class="align-center" style="width: 200px">
              {{ ticket.ticketId }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ ticket.fullName }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ formatedDateTime(ticket.bookingDate) }}
            </td>
            <td class="align-center" style="width: 400px">
              {{ ticket.movieTitle }}
            </td>
            <td class="align-center" style="width: 100px">
              {{ ticket.position }}
            </td>
            <td class="align-center" style="width: 200px">
              {{ ticket.ticketPrice }}
            </td>

            <td class="movie-list--actions" style="width: 220px">
              <span
                class="btn--checkout"
                v-if="ticket.paymentStatus"
                @click="changeStatusCheckout(0, index, ticket.bookingId)"
                >Đã thanh toán</span
              >
              <span
                class="btn--notcheckout"
                v-else
                @click="changeStatusCheckout(1, index, ticket.bookingId)"
                >Chưa thanh toán</span
              >
              <span
                class="btn-received"
                v-if="ticket.status"
                @click="changeStatus(0, index, ticket.ticketId)"
                >Đã nhận</span
              >
              <span
                class="btn--notreceived"
                v-else
                @click="changeStatus(1, index, ticket.ticketId)"
                >Chưa nhận</span
              >
            </td>
          </tr>
        </tbody>
      </table>
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

<script>
import instance from "../../common/instance";
import { delay, formatDateTime } from "@/common/common";
import debounce from "lodash/debounce";
export default {
  name: "TicketList",
  components: {},
  data() {
    return {
      ticketList: [],
      movieList: [],
      cinemaList: [],
      selectedTicketIds: [],
      isShowToastSuccess: false,
      isShowToastError: false,
      labelToastSuccess: "",
      labelToastError: "",
      isShowStatusList: [],
      filterBookingIdInput: "",
    };
  },
  watch: {
    filterBookingIdInput: debounce(async function () {
      await this.getTicketList();
    }, 1000),
  },
  async created() {
    await this.getTicketList();
  },
  methods: {
    async getTicketList() {
      console.log(124)
      this.$store.commit("showMask");
      await instance
        .get(`Ticket/GetByBookingId?bookingId=${this.filterBookingIdInput}`)
        .then(async (res) => {
          await delay(500);
          this.$store.commit("hideMask");
          this.ticketList = res.data;
        })
        .catch((err) => console.log(err));
    },
    async deleteScreening() {
      let success = false;
      const idsToDelete = this.selectedTicketIds.map((item) => item.toString());

      // Gửi yêu cầu DELETE với dữ liệu dạng JSON
      await instance
        .delete("Ticket", { data: idsToDelete })
        .then(() => {
          success = true;
        })
        .catch((err) => console.log(err));

      if (success) {
        this.getTicketList();
        this.showToastSuccess("Xóa thành công");
      }
    },
    clickOnCheckbox(index, id) {
      if (this.$refs["checkbox"][index].isChecked) {
        this.selectedTicketIds = this.selectedTicketIds.filter((idMovie) => {
          return id !== idMovie;
        });
      } else {
        this.selectedTicketIds.push(id);
      }
      this.$refs["checkbox"][index].isChecked =
        !this.$refs["checkbox"][index].isChecked;
      if (this.ticketList.length === this.selectedTicketIds.length) {
        this.$refs["checkbox-all"].isChecked = true;
      } else {
        this.$refs["checkbox-all"].isChecked = false;
      }
    },

    handleScroller() {
      this.$refs.tableHeader.scrollLeft = this.$refs.tableContent.scrollLeft;
    },
    showToastSuccess(label) {
      this.isShowToastSuccess = true;
      this.labelToastSuccess = label;
      // Ẩn sau 3s
      setTimeout(() => {
        this.isShowToastSuccess = false;
      }, 3000);
    },
    showToastError(label) {
      this.isShowToastError = true;
      this.labelToastError = label;
      // Ẩn sau 3s
      setTimeout(() => {
        this.isShowToastError = false;
      }, 3000);
    },
    formatedDateTime(date) {
      return formatDateTime(date);
    },
    async changeStatusCheckout(status, index, id) {
      if (status == 1) {
        let res = confirm(
          "Bạn có muốn thay đổi trạng thái từ 'Chưa thanh toán' sang 'Đã thanh toán?'"
        );
        if (res) {
          this.updateStatusBooking(id, status);
        }
      } else if (status == 0) {
        let res = confirm(
          "Bạn có muốn thay đổi trạng thái từ 'Đã thanh toán' sang 'Chưa thanh toán?'"
        );
        if (res) {
          this.updateStatusBooking(id, status);
        }
      }
    },
    async changeStatus(status, index, id) {
      if (status == 1) {
        let res = confirm(
          "Bạn có muốn thay đổi trạng thái từ 'Chưa nhận' sang 'Đã nhận?'"
        );
        if (res) {
          await this.updateStatusTicket(id, status);
        }
      } else if (status == 0) {
        let res = confirm(
          "Bạn có muốn thay đổi trạng thái từ 'Đã nhận' sang 'Chưa nhận?'"
        );
        if (res) {
          await this.updateStatusTicket(id, status);
        }
      }
    },
    async updateStatusTicket(id, status) {
      await instance
        .put(`Ticket/Change?ticketId=${id}&status=${status}`)
        .then(async () => {
          await this.getTicketList();
        })
        .catch((err) => {
          console.log(err);
        });
    },
    async updateStatusBooking(id, status) {
      await instance
        .put(`Booking/Change?bookingId=${id}&status=${status}`)
        .then(async () => {
          await this.getTicketList();
        })
        .catch((err) => {
          console.log(err);
        });
    },
  },
};
</script>

<style scoped>
@import url(../../css/views/tickets/ticket-list.css);
</style>
