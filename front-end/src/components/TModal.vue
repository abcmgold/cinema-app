<template>
    <div id="modalAdd" class="modal" style="display: block">
        
        <div class="modal__content">
            <div class="modal__icon--close" @click="this.$emit('closeModal')">
                <div class="icon--close"></div>
            </div>
            <slot></slot>
        </div>
    </div>
</template>

<script scoped>
export default {
    name: 'TModal',
};
</script>

<style scoped>
@import url(../css/components/modal.css);
@import url(../css/components/icon.css);
</style>
