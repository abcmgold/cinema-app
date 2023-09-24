<template>
    <div>
        <div :class="icon"></div>
    </div>
</template>

<script scoped>
export default {
    name: "MISAIcon",
    props: {
        icon: String
    },
}
</script>

<style scoped>
@import url(../css/components/modal.css);
</style>