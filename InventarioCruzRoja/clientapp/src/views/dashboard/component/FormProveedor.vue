<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="text-h3 font-weight-light">
          {{ modeEdit ? "Editar" : "Crear" }} Proveedor
        </div>
      </template>
      <v-form
        id="formProveedor"
        ref="form"
      >
        <validation-provider
          v-slot="{errors}"
          name="nombre"
          rules="required|min:2|max:50"
        >
          <v-text-field
            v-model="proveedor.nombre"
            label="Nombre"
            name="nombre"
            prepend-icon="mdi-home-plus"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="direccion"
          rules="required|min:2|max:300"
        >
          <v-text-field
            v-model="proveedor.direccion"
            label="Dirección"
            name="direccion"
            prepend-icon="mdi-map-marker-radius"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="telefonoFijo1"
          rules="max:15"
        >
          <vue-tel-input-vuetify
            v-model="proveedor.telefonoFijo1"
            label="Teléfono #1"
            name="telefonoFijo1"
            placeholder="Introduzca un número de teléfono"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="telefonoFijo2"
          rules="max:15"
        >
          <vue-tel-input-vuetify
            v-model="proveedor.telefonoFijo2"
            label="Teléfono #2"
            name="telefonoFijo2"
            placeholder="Introduzca un número de teléfono"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="sitioWeb"
          rules="url:{ require_protocol: true }|max:300"
        >
          <v-text-field
            v-model="proveedor.sitioWeb"
            label="Sitio Web"
            name="sitoWeb"
            prepend-icon="mdi-web"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="email"
          rules="email|max:100"
        >
          <v-text-field
            v-model="proveedor.email"
            label="Email"
            name="email"
            prepend-icon="mdi-at"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <v-col>
          <contactos-tabla
            v-model="proveedor.contactos"
            :proveedor-id="proveedor.id"
          />
        </v-col>
        <validation-provider
          v-slot="{errors}"
          name="estado"
          rules="required"
        >
          <v-select
            v-model="proveedor.estadoId"
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
          form="formProveedor"
          :disabled="invalid || ajaxInProgress"
          :small="$vuetify.breakpoint.xsOnly"
          @click="save"
        >
          {{ modeEdit ? "Editar" : "Crear" }}
        </v-btn>
        <v-btn
          color="error"
          class="mr-0"
          form="formProveedor"
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
  import ContactosTabla from './ContactosTabla.vue'

  export default {
    components: {
      ContactosTabla,
    },
    props: {
      modeEdit: {
        type: Boolean,
        default: false,
      },
      ajaxInProgress: {
        type: Boolean,
        default: false,
      },
      proveedor: {
        type: Object,
        default: function () {
          return {
            id: 0,
            nombre: '',
            direccion: '',
            telefonoFijo1: '',
            telefonoFijo2: '',
            sitioWeb: '',
            email: null,
            contactos: [],
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
