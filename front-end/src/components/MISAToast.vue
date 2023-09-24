<template>
    <div class = "toast">
        <div :class = "['toast__icon', icon]"></div>
        <div class = "toast__text">{{label}}</div>
    </div>
</template>

<script scoped>
export default {
    name: "MISAToast",
    props: {
        icon: String,
        label: String
    },
}
</script>

<style scoped>
@import url(@/css/components/toast.css);
@import url(@/css/components/icon.css);

</style>