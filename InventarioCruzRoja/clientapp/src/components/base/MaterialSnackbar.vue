<template>
  <v-snackbar
    v-model="internalValue"
    class="v-snackbar--material"
    v-bind="{
      ...$attrs,
      'color': 'transparent'
    }"
  >
    <v-row no-gutters>
      <v-col
        cols="12"
      >
        <base-material-alert
          v-model="internalValue"
          :color="$attrs.color"
          :dismissible="dismissible"
          :type="type"
          class="ma-0"
          dark
        >
          <slot />
        </base-material-alert>
      </v-col>
    </v-row>
  </v-snackbar>
</template>
<script>
  export default {
    name: 'BaseMaterialSnackbar',

    props: {
      dismissible: {
        type: Boolean,
        default: true,
      },
      type: {
        type: String,
        default: '',
      },
      value: Boolean,
    },

    data () {
      return {
        internalValue: this.value,
      }
    },

    watch: {
      internalValue (val, oldVal) {
        if (val === oldVal) return

        this.$emit('input', val)
      },
      value (val, oldVal) {
        if (val === oldVal) return

        this.internalValue = val
      },
    },
  }
</script>

<style lang="sass">
  .v-snackbar--material
    margin-top: 32px
    margin-bottom: 32px

    .v-alert--material,
    .v-snack__wrapper
      border-radius: 4px

    .v-snack__content
      overflow: visible
      padding: 0
</style>
