<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="display-2 font-weight-light">
          {{ modeEdit ? "Editar" : "Registrar" }} Producto
        </div>
      </template>
      <v-form
        id="formProducto"
        ref="form"
      >
        <v-container class="py-0">
          <v-row>
            <v-col md="4">
              <validation-provider
                v-slot="{errors}"
                name="codigo"
                rules="required|min:1|max:20"
              >
                <v-text-field
                  v-model="producto.id"
                  label="Código"
                  name="codigo"
                  prepend-icon="mdi-barcode"
                  type="text"
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
            <v-col md="4">
              <validation-provider
                v-slot="{errors}"
                name="nombre"
                rules="required|min:2|max:200"
              >
                <v-text-field
                  v-model="producto.nombre"
                  label="Nombre"
                  name="nombre"
                  prepend-icon="mdi-form-textbox"
                  type="text"
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
          </v-row>
          <v-row>
            <v-col md="4">
              <validation-provider
                v-slot="{errors}"
                name="modelo"
                rules="max:100"
              >
                <v-text-field
                  v-model="producto.modelo"
                  label="Modelo"
                  name="modelo"
                  prepend-icon="mdi-form-textbox"
                  type="text"
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
            <v-col md="4">
              <validation-provider
                v-slot="{errors}"
                name="presentacion"
                rules="max:100"
              >
                <v-text-field
                  v-model="producto.presentacion"
                  label="Presentación"
                  name="presentacion"
                  prepend-icon="mdi-form-textbox"
                  type="text"
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
          </v-row>
          <v-row>
            <v-col>
              <validation-provider
                v-slot="{errors}"
                name="observaciones"
                rules="max:1000"
              >
                <v-text-field
                  v-model="producto.observaciones"
                  label="Observaciones"
                  name="observaciones"
                  prepend-icon="mdi-magnify"
                  type="text"
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
          </v-row>
          <validation-provider
            v-slot="{errors}"
            name="estado"
            rules="required"
          >
            <v-select
              v-model="producto.estadoId"
              :items="estados"
              item-text="nombre"
              item-value="id"
              chips
              label="Estado"
              outlined
              :error-messages="errors"
            />
          </validation-provider>
        </v-container>
      </v-form>
      <template v-slot:actions>
        <v-btn
          color="success"
          form="formProducto"
          :disabled="invalid || ajaxInProgress"
          :small="$vuetify.breakpoint.xsOnly"
          @click="save"
        >
          {{ modeEdit ? "Editar" : "Crear" }}
        </v-btn>
        <v-btn
          color="error"
          class="mr-0"
          form="formProducto"
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
      producto: {
        type: Object,
        default: function () {
          return {
            id: '',
            nombre: '',
            modelo: '',
            presentacion: '',
            descripcion: '',
            observaciones: '',
            fabricanteId: 0,
            sedeId: 0,
            estadoId: 0,
            costo: 0,
            stock: 0,
            imagenUrl: 0,
          }
        },
      },
    },
    data () {
      return {
      }
    },
  }
</script>

<style>

</style>
