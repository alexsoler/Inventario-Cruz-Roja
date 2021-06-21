<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="text-h3 font-weight-light">
          {{ modeEdit ? "Editar" : "Crear" }} Usuario
        </div>
      </template>
      <v-form
        id="formUser"
        ref="form"
      >
        <validation-provider
          v-slot="{errors}"
          name="Usuario"
          rules="required|min:4|max:20"
        >
          <v-text-field
            v-model="user.username"
            label="Usuario"
            name="Username"
            prepend-icon="mdi-account"
            :disabled="modeEdit"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="Roles"
          rules="required"
        >
          <v-select
            v-model="user.roles"
            :items="roles"
            chips
            label="Roles"
            multiple
            outlined
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="Contraseña"
          :rules="validationPassword"
          vid="confirmation"
        >
          <v-text-field
            v-model="password"
            label="Contraseña"
            name="password"
            prepend-icon="mdi-lock"
            type="password"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="Confirmar contraseña"
          :rules="validationConfirmPassword"
        >
          <v-text-field
            v-model="confirmPassword"
            label="Confirmar contraseña"
            name="confirmPassword"
            prepend-icon="mdi-lock-check"
            type="password"
            :error-messages="errors"
          />
        </validation-provider>
      </v-form>
      <template v-slot:actions>
        <v-btn
          color="success"
          form="formUser"
          :disabled="invalid || ajaxInProgress"
          :small="$vuetify.breakpoint.xsOnly"
          @click="save"
        >
          {{ modeEdit ? "Editar" : "Crear" }}
        </v-btn>
        <v-btn
          color="error"
          class="mr-0"
          form="formUser"
          :small="$vuetify.breakpoint.xsOnly"
          @click="reset"
        >
          Reiniciar
        </v-btn>
        <v-spacer />
        <v-btn
          color="success darken-1"
          text
          :small="$vuetify.breakpoint.xsOnly"
          @click="close"
        >
          Cerrar
        </v-btn>
      </template>
    </base-material-card>
  </validation-observer>
</template>

<script>
  import AuthService from '@/services/auth.service'

  export default {
    props: {
      modeEdit: {
        type: Boolean,
        default: false,
      },
      ajaxInProgress: {
        type: Boolean,
        default: false,
      },
      user: {
        type: Object,
        default: function () {
          return {
            id: 0,
            username: '',
            roles: [],
          }
        },
      },
      statusDialog: {
        type: Boolean,
      },
    },
    data () {
      return {
        password: '',
        confirmPassword: '',
        roles: [],
      }
    },
    computed: {
      validationPassword () {
        return this.modeEdit ? 'min:6|max:50' : 'required|min:6|max:50'
      },
      validationConfirmPassword () {
        return this.modeEdit ? 'confirmed:confirmation' : 'required|confirmed:confirmation'
      },
    },
    watch: {
      statusDialog (val) {
        val || this.resetPassword()
      },
    },
    async created () {
      const response = await AuthService.getAllRoles()
      if (response.success) {
        this.roles = response.data
      }
    },
    methods: {
      close () {
        this.$emit('close')
        this.reset()
      },
      save () {
        this.$emit('save', this.password)
      },
      reset () {
        this.$refs.form.reset()
        this.$refs.observerValidate.reset()
      },
      resetPassword () {
        this.password = ''
        this.confirmPassword = ''
        this.$refs.observerValidate.reset()
      },
    },
  }
</script>

<style lang="sass" scoped>

</style>
