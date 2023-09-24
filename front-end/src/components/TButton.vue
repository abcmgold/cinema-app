<template>
    <button class="btn" :class="[this.class]">{{ label }}</button>
</template>

<script scoped>
export default {
    name: "TButton",
    props: {
        label: String,
        class: String
    }
}

</script>

<style scoped>
@import url(../css/components/button.css);

</style>
