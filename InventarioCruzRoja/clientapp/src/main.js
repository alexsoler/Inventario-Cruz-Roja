// =========================================================
// * Vuetify Material Dashboard - v2.1.0
// =========================================================
//
// * Product Page: https://www.creative-tim.com/product/vuetify-material-dashboard
// * Copyright 2019 Creative Tim (https://www.creative-tim.com)
//
// * Coded by Creative Tim
//
// =========================================================
//
// * The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

import Vue from 'vue'
import App from './App.vue'
import { router } from './router'
import store from './store'
import './plugins/base'
import './plugins/chartist'
import './plugins/vee-validate'
import ChatHub from './plugins/chat-hub'
import vuetify from './plugins/vuetify'
import i18n from './i18n'
import VueTelInputVuetify from 'vue-tel-input-vuetify/lib'
import VueSweetalert2 from 'vue-sweetalert2'
import 'sweetalert2/dist/sweetalert2.min.css'
import './assets/css/main.css'
import moment from 'moment'

Vue.use(VueSweetalert2)
Vue.use(ChatHub)
Vue.use(VueTelInputVuetify, {
  vuetify,
})

Vue.filter('formatDate', function (value) {
  if (value) {
      return moment(String(value)).format('DD/MM/YYYY hh:mm')
  }
})

Vue.config.productionTip = false

Vue.filter('currency', function (value) {
  return 'L. ' + parseFloat(value).toFixed(2)
})

new Vue({
  router,
  store,
  vuetify,
  i18n,
  render: h => h(App),
}).$mount('#app')
