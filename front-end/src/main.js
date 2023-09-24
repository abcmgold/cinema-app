import { createApp } from 'vue'
import App from './App.vue'
import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import ENUM from './common/enum';
import { MISAResource } from './common/resource';

import TButton from './components/TButton.vue'
import MISAButton from './components/MISAButton.vue'
import TCardFilm from './components/TCardFilm.vue'
import TModal from './components/TModal.vue'
import TSeat from './components/TSeat.vue'
import MISATextInput from './components/MISATextInput.vue'
import MISAIcon from './components/MISAIcon.vue'
import MISAToast from './components/MISAToast.vue'
import MISAModal from './components/MISAModal.vue'
import MISACombobox from './components/MISACombobox.vue'
import MISANumberInput from './components/MISANumberInput.vue'
import MISADialog from './components/MISADialog.vue'
import MISACheckbox from './components/MISACheckbox.vue'
import MISAPagination from './components/MISAPagination.vue'
import MISAValidateText from './components/MISAValidateText.vue'
import MISAContextMenu from './components/MISAContextMenu.vue'
import MISADatePicker from './components/MISADatepicker.vue'


import vueRouter from './router';
import store from './store'


const app = createApp(App);
app.component('t-button', TButton)
app.component('m-button', MISAButton)
app.component('t-card', TCardFilm)
app.component('t-modal', TModal)
app.component('t-seat', TSeat)
app.component('m-text-input', MISATextInput)
app.component('m-icon', MISAIcon)
app.component('m-toast', MISAToast)
app.component('m-modal', MISAModal)
app.component('m-combo-box', MISACombobox)
app.component('m-number-input', MISANumberInput)
app.component('m-dialog', MISADialog)
app.component('m-checkbox', MISACheckbox)
app.component('m-pagination', MISAPagination)
app.component('m-validate-text', MISAValidateText)
app.component('m-context-menu', MISAContextMenu)
app.component('m-date-picker', MISADatePicker)

app.use(ElementPlus)
app.use(vueRouter)
app.use(store)

app.config.globalProperties.$_MISAResource = MISAResource;
app.config.globalProperties.$_MISAEnum = ENUM;

app.mount('#app')

