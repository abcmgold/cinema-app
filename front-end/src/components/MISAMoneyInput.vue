<template>
    <div class="number-field__input">
        <input
            ref="myInputNumber"
            type="text"
            class="number-field__input--value"
            :class="[{ 'number-field__input--error': error }, individualClass, , { 'text__input--icon': isShowIcon }]"
            v-model="currentvalue"
            v-on:blur="onBlurFunction"
            @input="
                () => {
                    checkMaxLength();
                    onChangeFunction();
                }
            "
            @keydown="blockAlphabets"
            v-on:focus="highlightInput"
        />
        <div class="number-field__input--icons" v-if="isShowIcon">
            <div class="number-field__input--icon--up" @click="this.increasingValue"></div>
            <div class="number-field__input--icon--down" @click="this.decreasingValue"></div>
        </div>
    </div>
</template>

<script>
import { formatMoney, unformatMoney } from '@/common/common';
export default {
    name: 'MISAMoneyInput',
    props: {
        label: String,
        individualClass: String,
        modelValue: Number,
        isRequired: Boolean,
        errorMsg: String,
        isShowIcon: Boolean,
        type: String,
        maxLength: Number,
    },
    data() {
        return {
            error: false,
            currentvalue: null,
        };
    },
    created() {
        this.currentvalue = this.formattedMoney(this.modelValue);
    },
    watch: {
        currentvalue: function (newValue) {
            newValue = this.unformattedMoney(newValue);
            this.$emit('update:modelValue', newValue);
            this.currentvalue = this.formattedMoney(newValue);

            this.onChangeFunction();
        },
        modelValue: function (newValue) {
            this.currentvalue = this.formattedMoney(newValue);
        },
    },
    emit: ['update:modelValue'],
    methods: {
        /*
         * Bôi đậm giá trị ô input khi click vào ô đó
         * Author: BATUAN (14/06/2023)
         */
        highlightInput: function () {
            // Bôi đen nội dung trong ô input khi tập trung
            this.$refs.myInputNumber.select();
        },
        /*
         * Sự kiện ngăn chặn người dùng nhập các kí tự không hợp lệ vào ô input number
         * Author: BATUAN (14/06/2023)
         */
        blockAlphabets(event) {
            // Lấy giá trị của phím được nhấn
            const keyCode = event.keyCode || event.which;
            if (event.ctrlKey && (keyCode === 65 || keyCode === 86 || keyCode === 67)) {
                // Cho phép sự kiện xảy ra
                return;
            }

            // Kiểm tra nếu ký tự không phải số
            if (!/^[0-9]+$/.test(keyCode) && keyCode !== 8 && keyCode !== 37 && keyCode !== 39 && keyCode !== 9) {
                event.preventDefault(); // Chặn sự kiện nhập
            }
        },
        /*
         * Kiểm tra xem độ dài ô input vượt quá giới hạn hay chưa
         * Author: BATUAN (14/06/2023)
         */
        checkMaxLength() {
            if (this.currentvalue && this.currentvalue.length > this.maxLength) {
                // Giá trị vượt quá maxLength
                this.currentvalue = this.currentvalue.slice(0, this.maxLength);
            }
        },
        /*
         * Tăng giá trị ô input khi ấn vào mũi tên tăng
         * Author: BATUAN (14/06/2023)
         */
        increasingValue() {
            this.$emit('update:modelValue', Number(this.modelValue) + 1);
        },
        /*
         * Giảm giá trị ô input khi ấn vào mũi tên giảm
         * Author: BATUAN (14/06/2023)
         */
        decreasingValue() {
            if (this.modelValue > 0) {
                this.$emit('update:modelValue', Number(this.modelValue) - 1);
            }
        },
        /*
         * Sự kiện khi blur khỏi ô input
         * Author: BATUAN (14/06/2023)
         */
        onBlurFunction() {
            if (this.modelValue === undefined || this.modelValue == '') {
                this.error = true;
                if (this.$parent.showErrorMessage) {
                    this.$parent.showErrorMessage();
                }
            }
        },
        /*
         * Sự kiện giá trị ô input thay đổi
         * Author: BATUAN (14/06/2023)
         */
        onChangeFunction() {
            this.error = false;
            if (this.$parent.hideErrorMessage) {
                this.$parent.hideErrorMessage();
            }
        },
        /*
         * Hàm format tiền
         * Author: BATUAN (14/06/2023)
         */
        formattedMoney(value) {
            return formatMoney(value);
        },
        /*
         * Hàm bỏ format tiền
         * Author: BATUAN (14/06/2023)
         */
        unformattedMoney(money) {
            return unformatMoney(money);
        },
        /*
         * Sự kiện tự động focus vào ô input
         * Author: BATUAN (14/06/2023)
         */
        autoFocus() {
            this.$refs.myInputNumber.focus();
        },
    },
};
</script>

<style scoped>
@import url(@/css/components/numberfield.css);
.number-field__input--value {
    appearance: none;
    -moz-appearance: textfield; /* Firefox */
}

.number-field__input--value::-webkit-inner-spin-button,
.number-field__input--value::-webkit-outer-spin-button {
    -webkit-appearance: none;
    margin: 0;
}
</style>
