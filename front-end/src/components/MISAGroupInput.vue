<template>
    <div class="input-group">
        <div class="input__text--label">
            {{ text }}
            <div class="input__icon--force" v-show="isForce">*</div>
        </div>
        <slot @showErrorMessage="showErrorMessage"></slot>
        <m-validate-text
            ref="validateText"
            :isEmptyField="this.isShowMessage"
            :message="this.message"
        ></m-validate-text>
    </div>
</template>

<script>
export default {
    name: 'MISAGroupInput',
    props: {
        text: String,
        isForce: Boolean,
        message: String,
    },
    data() {
        return {
            isShowMessage: false,
        };
    },
    methods: {
        /*
         * Sự kiện hiển thị error message dưới các ô input trong các form
         * Author: BATUAN (14/06/2023)
         */
        showErrorMessage() {
            this.isShowMessage = true;
        },
         /*
         * Sự kiện ẩn error message dưới các ô input trong các form
         * Author: BATUAN (14/06/2023)
         */
        hideErrorMessage() {
            this.isShowMessage = false;
        },
    },
};
</script>

<style scoped>
@import url(@/css/components/inputgroup.css);
</style>
