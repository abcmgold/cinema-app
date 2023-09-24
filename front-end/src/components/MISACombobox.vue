<template>
  <el-select
    ref="myComboBox"
    :disabled="disabled"
    v-model="selectedPropertyTypeName"
    :class="{ 'el-select--error': error }"
    class="combo-box"
    clearable
    :isRequired="isRequired"
    v-on:blur="onBlurFunction"
    v-on:change="onChangeFunction"
    :placeholder="placeholder"
    v-on:focus="
      () => {
        if (this.$parent.hideErrorMessage) {
          this.$parent.hideErrorMessage();
        }
      }
    "
    :filterable="this.filterable"
    :value="modelValue"
    @change="
      $emit(
        'update:modelValue',
        this.dataSelect.find((code) => {
          return code.value == this.selectedPropertyTypeName;
        })
          ? this.dataSelect.find((code) => {
              return code.value == this.selectedPropertyTypeName;
            }).value
          : ''
      )
    "
  >
    <el-option
      v-for="code in this.dataSelect"
      :key="code.value"
      :label="code.label"
      :value="code.value"
    ></el-option>
  </el-select>
</template>

<script scoped>
export default {
  data() {
    return { selectedPropertyTypeName: "", error: false };
  },
  name: "MISACombobox",
  props: {
    dataSelect: Array,
    modelValue: String,
    isRequired: Boolean,
    placeholder: String,
    errorMsg: String,
    type: String,
    filterable: Boolean,
  },
  mounted() {
    this.selectedPropertyTypeName = this.modelValue;
  },
  watch: {},
  methods: {
    /*
     * Sự kiện khi blur khỏ ô input
     * Author: BATUAN (14/06/2023)
     */
    onBlurFunction() {
      if (this.isRequired) {
        if (
          this.modelValue === undefined ||
          this.modelValue == "" ||
          this.modelValue.trim().length == 0
        ) {
          this.error = true;
          // this.$refs.inputValue.value = this.$refs.inputValue.value.trim();
          if (this.$parent.showErrorMessage) {
            this.$parent.showErrorMessage();
          }
        }
      }
    },
    /*
     * Sự kiện khi ô input thay đổi
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
      this.$refs.myComboBox.focus();
    },
  },
  emit: ["update:modelValue"],
};
</script>

<style>
@import url(@/css/components/combobox.css);
</style>
