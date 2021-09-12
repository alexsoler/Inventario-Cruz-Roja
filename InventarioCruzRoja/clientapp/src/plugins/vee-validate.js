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
extend('url', {
  validate (value) {
    var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
      '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.)+[a-z]{2,}|' + // domain name
      '((\\d{1,3}\\.){3}\\d{1,3}))' + // OR ip (v4) address
      '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + // port and path
      '(\\?[;&a-z\\d%_.~+=-]*)?' + // query string
      '(\\#[-a-z\\d_]*)?$', 'i') // fragment locator
    return pattern.test(value)
  },
  message: 'El campo no es una URL válida.',
})

Vue.component('validation-provider', ValidationProvider)
Vue.component('validation-observer', ValidationObserver)
