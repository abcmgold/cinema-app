<template>
  <input
    v-if="type != 'password'"
    ref="inputValue"
    type="text-field"
    :class="[
      'text__input',
      individualClass,
      { 'text__input--error': error, 'text__input--not-allowed': isDisabled },
    ]"
    :value="modelValue"
    @input="
      ($event) => {
        $emit('update:modelValue', $event.target.value);
        onChangeFunction();
      }
    "
    :placeholder="placeholder"
    :disabled="isDisabled"
    v-on:blur="onBlurFunction"
    @focus="highlightInput"
  />
  <input
    v-if="type == 'password'"
    ref="inputValue"
    type="password"
    :class="[
      'text__input',
      individualClass,
      { 'text__input--error': error, 'text__input--not-allowed': isDisabled },
    ]"
    :value="modelValue"
    @input="
      ($event) => {
        $emit('update:modelValue', $event.target.value);
        onChangeFunction();
      }
    "
    :placeholder="placeholder"
    :disabled="isDisabled"
    v-on:blur="onBlurFunction"
    @focus="highlightInput"
  />
</template>

<script>
export default {
  name: "MISATextInput",
  props: {
    placeholder: String,
    individualClass: String,
    isDisabled: Boolean,
    modelValue: String,
    onBlur: Function,
    isRequired: Boolean,
    errorMsg: String,
    type: String,
  },
  data() {
    return {
      error: false,
    };
  },

  methods: {
    /*
     * Sự kiện khi click vào ô input thì giá trị trong ô đó được bôi đậm
     * Author: BATUAN (14/06/2023)
     */
    highlightInput: function () {
      // Bôi đen nội dung trong ô input khi tập trung
      this.$refs.inputValue.select();
    },
    /*
     * Sự kiện khi blur khỏi ô input
     * Author: BATUAN (14/06/2023)
     */
    onBlurFunction() {
      if (this.isRequired) {
        if (this.modelValue === undefined || this.modelValue === "") {
          this.error = true;
          if (this.$parent.showErrorMessage) {
            this.$parent.showErrorMessage();
          }
        }
      }
    },
    /*
     * Sự kiện khi giá trị ô input thay đổi
     * Author: BATUAN (14/06/2023)
     */
    onChangeFunction() {
      this.error = false;
      if (this.$parent.hideErrorMessage) {
        this.$parent.hideErrorMessage();
      }
    },
    /*
     * Sự kiện tự động focus vào ô input
     * Author: BATUAN (14/06/2023)
     */
    autoFocus() {
      this.$refs.inputValue.focus();
    },
  },
  emits: ["update:modelValue"],
};
</script>

<style scoped>
@import url(@/css/components/textfield.css);
</style>
