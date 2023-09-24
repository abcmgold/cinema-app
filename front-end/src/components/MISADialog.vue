<template>
    <div class="dialog">
        <div class="dialog__discription">
            <div class="dialog__discription--icon">
                <div class="icon--warning"></div>
            </div>
            <div class="dialog__discription--text"><strong>{{beginText}}</strong> {{ text }} <strong>{{endText}}</strong></div>
        </div>
        <div class="dialog__list-btns">
            <div class="dialog__btn">
                <button class="btn btn--sub" v-if="firstBtnLabel" @click="firstBtnFunction">{{ firstBtnLabel }}</button>
                <button class="btn btn--primary" v-if="thirdBtnLabel" @click="thirdBtnFunction">{{ thirdBtnLabel }}</button>
            </div>
        </div>
    </div>
</template>

<script scoped>
export default {
    name: 'MISADialog',
    props: {
        text: String,
        firstBtnLabel: String,
        secondBtnLabel: String,
        thirdBtnLabel: String,
        firstBtnFunction: Function,
        secondBtnFunction: Function,
        thirdBtnFunction: Function,
        saveChanges: Function,
        endText: String,
        beginText: String
    },
};
</script>

<style>
@import url(../css/components/button.css);
@import url(../css/components/icon.css);
</style>
