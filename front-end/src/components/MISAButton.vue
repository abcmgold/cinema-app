<template>
        <button :class="['btn', individualClass]">
            <div v-if="icon" :class="[icon]"></div>
            <span v-if="label">{{ label }}</span>
        </button>
</template>

<script>
export default {
    name: 'MISAButton',
    props: {
        label: String,
        individualClass: String,
        icon: String,
    },
};
</script>

<style scoped>
@import url(../css/components/button.css);
@import url(../css/components/icon.css);
</style>
