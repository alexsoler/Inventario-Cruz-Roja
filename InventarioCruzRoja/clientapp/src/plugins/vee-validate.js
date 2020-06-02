import Vue from 'vue'
import es from 'vee-validate/dist/locale/es'
import {
  extend,
  ValidationObserver,
  ValidationProvider,
  localize,
} from 'vee-validate'
import {
  email,
  max,
  min,
  numeric,
  required,
  confirmed,
} from 'vee-validate/dist/rules'

localize('es', es)

extend('email', email)
extend('max', max)
extend('min', min)
extend('numeric', numeric)
extend('required', required)
extend('confirmed', confirmed)

Vue.component('validation-provider', ValidationProvider)
Vue.component('validation-observer', ValidationObserver)
