<template>
    <div id="modalAdd" class="modal" style="display: block">
        <div class="modal__content">
            <slot></slot>
        </div>
    </div>
</template>

<script scoped>
export default {
    name: 'MISAModal',
};
</script>

<style scoped>
@import url(../css/components/modal.css);
</style>
