<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="display-2 font-weight-light">
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
          rules="required|max:20"
        >
          <v-text-field
            v-model="username"
            label="Usuario"
            name="Username"
            prepend-icon="mdi-account"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="Contraseña"
          rules="required|min:6|max:50"
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
          rules="confirmed:confirmation"
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
          :disabled="invalid"
        >
          {{ modeEdit ? "Editar" : "Crear" }}
        </v-btn>
        <v-btn
          color="error"
          class="mr-0"
          form="formUser"
          @click="$refs.form.reset()"
        >
          Reset
        </v-btn>
      </template>
    </base-material-card>
  </validation-observer>
</template>

<script>
  export default {
    props: {
      modeEdit: {
        type: Boolean,
        default: false,
      },
    },
    data () {
      return {
        username: '',
        password: '',
        confirmPassword: '',
      }
    },
  }
</script>

<style lang="sass" scoped>

</style>
