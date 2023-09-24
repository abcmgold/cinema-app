<template>
    <div class="input-group">
        <div class="input__text--label" :label="label">{{text}}
            <div class="input__icon--force" v-show="isForce">*</div>
        </div>
        <m-text-input :placeholder="placeholder" :individualClass="individualClass" 
            :value="value" :isDisabled="isDisabled">
        </m-text-input>
    </div>
</template>


<script>
export default {
    name: "MISAGroupText",
    props: {
        text: String,
        label: String,
        isForce: Boolean,
        placeholder: String,
        individualClass: String,
        value: String,
        isDisabled: Boolean,
    },
}
</script>

<style scoped>
@import url(@/css/components/inputgroup.css);
</style>