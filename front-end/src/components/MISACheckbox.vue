<template>
    <div @click.stop class="checkbox" :class="{ checked: isChecked }" >
    </div>
</template>

<script scoped>
export default {
    name: 'MISACheckbox',
    props: {
        checkedCheckbox: Function,
        uncheckedCheckbox: Function,
        selectAllRow: Function,
        unSelectAllRow: Function,
        id: String,
        type: String,
    },
    data() {
        return {
            isChecked: false,
        };
    },
    methods: {
        /*
         * Sự kiện khi toggle ô checkbox
         * Author: BATUAN (14/06/2023)
         */
        handleCheckboxChange() {
            // Xử lý khi checkbox được chọn
            if (!this.isChecked) {
                if (this.type == 'primary') {
                    this.isChecked = true;
                    this.$emit('selectAllRow');
                } else {
                    this.$emit('checkedCheckbox', this.id);
                }
            } else {
                // Xử lý khi checkbox bị bỏ chọn
                if (this.type == 'primary') {
                    this.isChecked = false;
                    this.$emit('unSelectAllRow');
                } else {
                    this.$emit('uncheckedCheckbox', this.id);
                }
            }
        },
    },
};
</script>

<style scoped>
@import url(../css/components/checkbox.css);
</style>
