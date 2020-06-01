import Vue from 'vue'
import VueI18n from 'vue-i18n'

import es from 'vuetify/lib/locale/es'

Vue.use(VueI18n)

const messages = {
  es: {
    ...require('@/locales/en.json'),
    $vuetify: es,
  },
}

export default new VueI18n({
  locale: process.env.VUE_APP_I18N_LOCALE || 'es',
  fallbackLocale: process.env.VUE_APP_I18N_FALLBACK_LOCALE || 'es',
  messages,
})
