<template>
    <div
        ref="contextMenu"
        class="context-menu"
        v-if="this.isShowContext"
        :style="{ top: `${position.y}px`, left: `${position.x}px` }"
    >
        <ul class="list-items">
            <li class="item" @click="this.$emit('showDetail')">
                <div class="item-icon icon--update"></div>
                <div class="item-content">Sửa</div>
            </li>
            <li class="item" @click="this.$emit('deleteOneRow')">
                <div class="item-icon icon--delete"></div>
                <div class="item-content">Xóa</div>
            </li>
            <li class="item" @click="this.$emit('showDuplicate')">
                <div class="item-icon icon--duplicate"></div>
                <div class="item-content">Nhân bản</div>
            </li>
        </ul>
    </div>
</template>

<script scoped>
export default {
    name: 'MISAContextMenu',
    props: {
        position: Object,
        isShowContext: Boolean,
    },
    created() {},
    updated() {
        if (this.$refs.contextMenu && this.$store.getters.getContextMenuSize.x != this.$refs.contextMenu.clientWidth ) {
            this.$nextTick(() => {
                this.$store.commit('setContextMenuSize', {
                    x: this.$refs.contextMenu.clientWidth,
                    y: this.$refs.contextMenu.clientHeight,
                });
            });
        }
    }
};
</script>

<style scoped>
@import url(@/css/components/contextmenu.css);
@import url(@/css/components/icon.css);
</style>
