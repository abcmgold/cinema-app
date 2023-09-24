<template>
    <div class="input__message" :class=" {'isEmptyField': isEmptyField}">{{ message }}</div>
</template>

<script scoped>
export default {
    name: 'MISAValidateText',
    props: {
        message: String,
        isEmptyField: Boolean
    }
};
</script>

<style scoped>
.input__message {
    font-size: 12px;
    color: red;
    margin-top: 2px;
    visibility: hidden;
}
.isEmptyField {
    visibility: visible;
}
</style>
