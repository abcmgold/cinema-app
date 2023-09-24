<template>
    <VueDatePicker
        ref="myDatePicker"
        v-model="modelValue"
        auto-apply
        text-input
        :state="this.notError"
        :day-names="this.MISAResource['vn-VI'].dayInDatePicker"
        locale="vi"
        @blur="onBlurFunction"
        @focus="onFocusFunction"
    ></VueDatePicker>
</template>


//        format="dd/MM/yyyy"
<script scoped>
import VueDatePicker from '@vuepic/vue-datepicker';
import '@vuepic/vue-datepicker/dist/main.css';
import { MISAResource } from '../common/resource';
export default {
    name: 'MISADatepicker',
    props: {
        type: String,
        isRequired: {
            type: Boolean,
            default: false,
        },
        errorMsg: String,
    },
    data() {
        return {
            notError: true,
            modelValue: new Date().toLocaleDateString(),
            MISAResource: MISAResource,
        };
    },
    components: {
        VueDatePicker,
    },
    watch: {
        modelValue: function () {
            this.notError = true;
            if (this.$parent.hideErrorMessage) {
                this.$parent.hideErrorMessage();
            }
        },
    },
    methods: {
        /*
         * Sự kiện khi blur khỏi components
         * Author: BATUAN (14/06/2023)
         */
        onBlurFunction() {
            if (!this.modelValue) {
                this.notError = false;
                if (this.$parent) {
                    this.$parent.showErrorMessage();
                }
            }
        },
        /*
         * Sự kiện khi giá trị thay đổi
         * Author: BATUAN (14/06/2023)
         */
        onFocusFunction() {
            this.notError = true;
        },
        /*
         * Sự kiện tự động focus vào ô input
         * Author: BATUAN (14/06/2023)
         */
        autoFocus() {
            this.$refs.myDatePicker.openMenu();
        },
    },
};
</script>

<style scoped>
@import url(../css/datepicker/datepicker.css);
</style>
