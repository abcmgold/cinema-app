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
        />
        <div class="number-field__input--icons" v-if="isShowIcon">
            <div class="number-field__input--icon--up" @click="this.increasingValue"></div>
            <div class="number-field__input--icon--down" @click="this.decreasingValue"></div>
        </div>
    </div>
</template>

<script>
import { formatRatio } from '@/common/common';
export default {
    name: 'MISANumberInput',
    props: {
        label: String,
        individualClass: String,
        modelValue: Number,
        isRequired: {
            type: Boolean,
            default: false,
        },
        errorMsg: String,
        isShowIcon: Boolean,
        type: String,
        maxLength: Number,
    },
    data() {
        return {
            error: false,
            currentvalue: 0,
        };
    },
    created() {
        if (this.modelValue) {
            this.currentvalue = this.formattedRatio(this.modelValue);
        } else {
            this.currentvalue = 0;
        }
    },
    mounted() {},
    watch: {
        currentvalue: function (newValue) {
            if (newValue) {
                newValue = parseFloat(newValue.replace(',', '.'));
            }
            this.$emit('update:modelValue', newValue);
            this.currentvalue = this.formattedRatio(this.modelValue);
            this.error = false;
            if (this.$parent.hideErrorMessage) {
                this.$parent.hideErrorMessage();
            }
        },
        modelValue: function (newValue) {
            this.currentvalue = this.formattedRatio(newValue);
        },
    },
    emit: ['update:modelValue'],
    methods: {
        /*
         * Hàm format tỷ lệ
         * Author: BATUAN (14/06/2023)
         */
        formattedRatio(value) {
            return formatRatio(value);
        },
        /*
         * Sự kiện ngăn chặn người dùng nhập các ksi tự không hợp lệ vào ô input number
         * Author: BATUAN (14/06/2023)
         */
        blockAlphabets(event) {
            // Lấy giá trị của phím được nhấn
            const keyCode = event.keyCode || event.which;
            const key = String.fromCharCode(keyCode);
            if (event.ctrlKey && (keyCode === 65 || keyCode === 86 || keyCode === 67)) {
                // Cho phép sự kiện xảy ra
                return;
            }

            // Kiểm tra nếu ký tự không phải số
            if (!/^[0-9]+$/.test(key) && keyCode !== 8 && keyCode !== 37 && keyCode !== 39 && keyCode !== 9) {
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
            if (this.isRequired) {
                if (this.modelValue === undefined || this.modelValue == '') {
                    this.error = true;
                    if (this.$parent.showErrorMessage) {
                        this.$parent.showErrorMessage();
                    }
                }
            }
        },
        /*
         * Sự kiện giá trị ô input thay đổi
         * Author: BATUAN (14/06/2023)
         */
        onChangeFunction() {
            if (this.isRequired) {
                this.error = false;
                if (this.$parent.hideErrorMessage) {
                    this.$parent.hideErrorMessage();
                }
            }
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
