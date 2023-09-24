<template>
    <div class="input-group">
        <div class="input__text--label" :label="label">{{text}}
            <div class="input__icon--force" v-show="isForce">*</div>
        </div>
        <m-number-input :individualClass="individualClass">
        </m-number-input>
    </div>
</template>


<script>
export default {
    name: "MISAGroupNumber",
    props: {
        text: String,
        label: String,
        isForce: Boolean,
        individualClass: String,
    },
}
</script>

<style scoped>
@import url(@/css/components/inputgroup.css);
</style>