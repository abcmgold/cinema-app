<template>
    <div class="seat" :class='{"selected": checked, "occupied": isOccupied, "vip": this.type==1, "normal": this.type==0}' @click="clickOnSeat">
        {{ this.name }}
    </div>
</template>

<script scoped>
export default {
    name: 'TSeat',
    props: {
        name: String,
        isOccupied: Number,
        type: Number,
    },
    data() {
        return {
            checked: false,
        }
    },
    methods: {
        clickOnSeat() {
            if (!this.isOccupied) {
                this.checked = !this.checked;
            }
        }
    }
};
</script>

<style scoped>
@import url(../css/components/seat.css);
</style>
