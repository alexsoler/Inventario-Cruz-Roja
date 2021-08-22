<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="text-h3 font-weight-light">
          {{ modeEdit ? "Editar" : "Crear" }} Categoria
        </div>
      </template>
      <v-form
        id="formCategoria"
        ref="form"
      >
        <validation-provider
          v-slot="{errors}"
          name="nombre"
          rules="required|min:2|max:50"
        >
          <v-text-field
            v-model="categoria.nombre"
            label="Nombre"
            name="nombre"
            prepend-icon="mdi-shape"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="descripcion"
          rules="max:300"
        >
          <v-text-field
            v-model="categoria.descripcion"
            label="Descripción"
            name="descripcion"
            prepend-icon="mdi-note-text"
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
            v-model="categoria.estadoId"
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
          form="formCategoria"
          :disabled="invalid || ajaxInProgress"
          :small="$vuetify.breakpoint.xsOnly"
          @click="save"
        >
          {{ modeEdit ? "Editar" : "Crear" }}
        </v-btn>
        <v-btn
          color="error"
          class="mr-0"
          form="formCategoria"
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
      categoria: {
        type: Object,
        default: function () {
          return {
            id: 0,
            nombre: '',
            descripcion: '',
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
