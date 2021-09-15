<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="text-h3 font-weight-light">
          {{ modeEdit ? "Editar" : "Registrar" }} Producto
        </div>
      </template>
      <v-form
        id="formProducto"
        ref="form"
      >
        <v-container class="py-0">
          <v-row>
            <v-col md="6">
              <validation-provider
                v-slot="{errors}"
                name="codigo"
                rules="required|min:1|max:20"
              >
                <v-text-field
                  v-model="producto.codigo"
                  label="Código"
                  name="codigo"
                  prepend-icon="mdi-barcode"
                  type="text"
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
            <v-col md="6">
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
            <v-col md="6">
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
            <v-col md="6">
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
          <v-row>
            <v-col
              cols="12"
              sm="4"
            >
              <validation-provider
                v-slot="{errors}"
                name="fabricante"
                rules="required"
              >
                <v-select
                  v-model="producto.fabricanteId"
                  :items="fabricantesGetter"
                  item-text="nombre"
                  item-value="id"
                  label="Fabricante"
                  outlined
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
            <v-col
              cols="12"
              sm="4"
            >
              <validation-provider
                v-slot="{errors}"
                name="categoria"
                rules="required"
              >
                <v-select
                  v-model="producto.categoriaId"
                  :items="categoriasGetter"
                  item-text="nombre"
                  item-value="id"
                  label="Categoria"
                  outlined
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
            <v-col
              cols="12"
              sm="4"
            >
              <validation-provider
                v-slot="{errors}"
                name="estado"
                rules="required"
              >
                <v-select
                  v-model="producto.estadoId"
                  :items="estadosGetter"
                  item-text="nombre"
                  item-value="id"
                  label="Estado"
                  outlined
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
          </v-row>
          <v-row>
            <v-col md="6">
              <validation-provider
                v-slot="{errors}"
                name="sede"
                rules="required"
              >
                <v-select
                  v-model="producto.sedes"
                  :items="sedesGetter"
                  item-text="nombre"
                  label="Sedes"
                  return-object
                  multiple
                  outlined
                  :error-messages="errors"
                >
                  <template v-slot:selection="{ item, index }">
                    <v-chip v-if="index === 0">
                      <span>{{ item.nombre }}</span>
                    </v-chip>
                    <span
                      v-if="index === 1"
                      class="grey--text text-caption"
                    >
                      (+{{ producto.sedes.length - 1 }} otros)
                    </span>
                  </template>
                  <template v-slot:prepend-item>
                    <v-list-item
                      ripple
                      @click="toggle"
                    >
                      <v-list-item-action>
                        <v-icon :color="producto.sedes.length > 0 ? $vuetify.theme.themes.light.primary : ''">
                          {{ icon }}
                        </v-icon>
                      </v-list-item-action>
                      <v-list-item-content>
                        <v-list-item-title>
                          Seleccionar todos
                        </v-list-item-title>
                      </v-list-item-content>
                    </v-list-item>
                    <v-divider class="mt-2" />
                  </template>
                </v-select>
              </validation-provider>
            </v-col>
            <v-col md="6">
              <validation-provider
                v-slot="{errors}"
                name="Costo"
                rules="required"
              >
                <v-text-field
                  v-model.number="producto.costo"
                  label="Costo"
                  name="costo"
                  prepend-icon="mdi-cash"
                  type="number"
                  prefix="L."
                  :error-messages="errors"
                />
              </validation-provider>
            </v-col>
          </v-row>
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
          Reiniciar
        </v-btn>
      </template>
    </base-material-card>
  </validation-observer>
</template>

<script>
  import { mapGetters } from 'vuex'

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
      producto: {
        type: Object,
        default: function () {
          return {
            id: 0,
            codigo: '',
            nombre: '',
            modelo: '',
            presentacion: '',
            descripcion: '',
            observaciones: '',
            fabricanteId: 0,
            categoriaId: 0,
            sedes: [],
            estadoId: 0,
            costo: 0,
            imagenUrl: 0,
          }
        },
      },
    },
    data () {
      return {
      }
    },
    computed: {
      ...mapGetters(['estadosGetter', 'fabricantesGetter', 'sedesGetter', 'categoriasGetter']),
      selectAllSedes () {
        return this.producto.sedes.length === this.sedesGetter.length
      },
      selectSomeSedes () {
        return this.producto.sedes.length > 0 && !this.selectAllSedes
      },
      icon () {
        if (this.selectAllSedes) return 'mdi-close-box'
        if (this.selectSomeSedes) return 'mdi-minus-box'
        return 'mdi-checkbox-blank-outline'
      },
    },
    methods: {
      save () {
        this.$emit('onSave', this.producto)
      },
      reset () {
        this.$refs.form.reset()
        this.$refs.observerValidate.reset()
      },
      toggle () {
        this.$nextTick(() => {
          if (this.selectAllSedes) {
            this.producto.sedes = []
          } else {
            this.producto.sedes = this.sedesGetter.slice()
          }
        })
      },
    },
  }
</script>

<style>

</style>
