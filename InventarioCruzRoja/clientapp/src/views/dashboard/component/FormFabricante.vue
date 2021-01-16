<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="display-2 font-weight-light">
          {{ modeEdit ? "Editar" : "Crear" }} Fabricante
        </div>
      </template>
      <v-form
        id="formFabricante"
        ref="form"
      >
        <validation-provider
          v-slot="{errors}"
          name="nombre"
          rules="required|min:2|max:100"
        >
          <v-text-field
            v-model="fabricante.nombre"
            label="Nombre"
            name="nombre"
            prepend-icon="mdi-tag"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="estado"
          rules="required"
        >
          <v-select
            v-model="fabricante.estadoId"
            :items="estados"
            item-text="nombre"
            item-value="id"
            chips
            label="Estado"
            outlined
            :error-messages="errors"
          />
        </validation-provider>
      </v-form>
      <template v-slot:actions>
        <v-btn
          color="success"
          form="formFabricante"
          :disabled="invalid || ajaxInProgress"
          :small="$vuetify.breakpoint.xsOnly"
          @click="save"
        >
          {{ modeEdit ? "Editar" : "Crear" }}
        </v-btn>
        <v-btn
          color="error"
          class="mr-0"
          form="formFabricante"
          :small="$vuetify.breakpoint.xsOnly"
          @click="reset"
        >
          Reset
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
      fabricante: {
        type: Object,
        default: function () {
          return {
            id: 0,
            nombre: '',
            estadoId: 1,
          }
        },
      },
    },
    data () {
      return {
        estados: [
          { id: 1, nombre: 'Activo' },
          { id: 2, nombre: 'Inactivo' },
        ],
      }
    },
    methods: {
      close () {
        this.$emit('close')
        this.reset()
      },
      save () {
        this.$emit('save')
      },
      reset () {
        this.$refs.form.reset()
        this.$refs.observerValidate.reset()
      },
    },
  }
</script>
