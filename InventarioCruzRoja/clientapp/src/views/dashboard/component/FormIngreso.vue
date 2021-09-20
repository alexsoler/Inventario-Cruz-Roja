<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="text-h3 font-weight-light">
          {{ modeEdit ? "Editar" : "Registrar" }} Ingreso
        </div>
      </template>
      <v-form
        id="formIngreso"
        ref="form"
      >
        <validation-provider
          v-slot="{errors}"
          name="producto"
          rules="required"
        >
          <v-autocomplete
            v-model="ingreso.productoId"
            :loading="isLoadingProductos"
            :items="productos"
            :search-input.sync="productoSearch"
            :filter="customFilterProducto"
            item-text="nombre"
            item-value="id"
            chips
            clearable
            cache-items
            flat
            hide-selected
            hide-details
            label="Buscar producto"
            name="producto"
            prepend-icon="mdi-clipboard-search"
            :error-messages="errors"
          >
            <template v-slot:no-data>
              <v-list-item>
                <v-list-item-title>
                  Buscar el <strong>producto</strong> por nombre o código
                </v-list-item-title>
              </v-list-item>
            </template>
            <template v-slot:selection="{ attr, on, item, selected }">
              <v-chip
                v-bind="attr"
                :input-value="selected"
                color="blue-grey"
                class="white--text"
                v-on="on"
              >
                <v-icon left>
                  mdi-package-variant
                </v-icon>
                <span v-text="item.nombre" />
              </v-chip>
            </template>
            <template v-slot:item="{ item }">
              <v-list-item-avatar
                color="indigo"
                class="text-h5 font-weight-light white--text"
              >
                {{ item.nombre.charAt(0) }}
              </v-list-item-avatar>
              <v-list-item-content>
                <v-list-item-title v-text="item.nombre" />
                <v-list-item-subtitle v-text="item.codigo" />
              </v-list-item-content>
              <v-list-item-action>
                <v-icon>mdi-package-variant</v-icon>
              </v-list-item-action>
            </template>
          </v-autocomplete>
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="proveedor"
          rules="required"
        >
          <v-autocomplete
            v-model="ingreso.proveedorId"
            :loading="isLoadingProveedores"
            :items="proveedores"
            :search-input.sync="proveedorSearch"
            item-text="nombre"
            item-value="id"
            chips
            clearable
            cache-items
            flat
            hide-selected
            hide-details
            label="Buscar proveedor"
            name="proveedor"
            prepend-icon="mdi-account-search"
            :error-messages="errors"
          >
            <template v-slot:no-data>
              <v-list-item>
                <v-list-item-title>
                  Buscar el <strong>proveedor</strong> por nombre
                </v-list-item-title>
              </v-list-item>
            </template>
            <template v-slot:selection="{ attr, on, item, selected }">
              <v-chip
                v-bind="attr"
                :input-value="selected"
                color="blue-grey"
                class="white--text"
                v-on="on"
              >
                <v-icon left>
                  mdi-truck
                </v-icon>
                <span v-text="item.nombre" />
              </v-chip>
            </template>
            <template v-slot:item="{ item }">
              <v-list-item-avatar
                color="indigo"
                class="text-h5 font-weight-light white--text"
              >
                {{ item.nombre.charAt(0) }}
              </v-list-item-avatar>
              <v-list-item-content>
                <v-list-item-title v-text="item.nombre" />
                <v-list-item-subtitle v-text="item.email" />
              </v-list-item-content>
              <v-list-item-action>
                <v-icon>mdi-truck</v-icon>
              </v-list-item-action>
            </template>
          </v-autocomplete>
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="sede"
          rules="required"
        >
          <v-select
            v-model="ingreso.sedeId"
            :items="sedesGetter"
            item-text="nombre"
            item-value="id"
            label="Sede"
            prepend-icon="mdi-home-plus"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="observaciones"
          rules="max:300"
        >
          <v-text-field
            v-model="ingreso.observaciones"
            label="Observaciones"
            name="observaciones"
            prepend-icon="mdi-magnify"
            type="text"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="cantidad"
          rules="required|min_value:1"
        >
          <v-text-field
            v-model.number="ingreso.cantidad"
            label="Cantidad"
            name="cantidad"
            prepend-icon="mdi-view-grid"
            type="number"
            :error-messages="errors"
          />
        </validation-provider>
      </v-form>
      <template v-slot:actions>
        <v-btn
          color="success"
          form="formIngreso"
          :disabled="invalid || ajaxInProgress"
          :small="$vuetify.breakpoint.xsOnly"
          @click="save"
        >
          {{ modeEdit ? "Editar" : "Registrar" }}
        </v-btn>
        <v-btn
          color="error"
          class="mr-0"
          form="formIngreso"
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
  import ProductosService from '@/services/productos.service'
  import ProveedoresService from '@/services/proveedores.service'
  import debounce from 'lodash/debounce'
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
      ingreso: {
        type: Object,
        default: function () {
          return {
            id: 0,
            productoId: 0,
            producto: '',
            proveedorId: 0,
            proveedor: '',
            sedeId: 0,
            sede: '',
            userId: 0,
            usuario: '',
            observaciones: '',
            anulado: false,
            cantidad: 0,
            fecha: '',
          }
        },
      },
    },
    data () {
      return {
        isLoadingProductos: false,
        productos: [],
        productoSearch: '',
        isLoadingProveedores: false,
        proveedores: [],
        proveedorSearch: '',
      }
    },
    computed: {
      ...mapGetters({
        sedesGetter: 'sedesGetter',
        userGetter: 'auth/user',
      }),
    },
    watch: {
      productoSearch (val) {
        if (!val || val.length < 3) return

        this.debouncedGetProducto()
      },
      proveedorSearch (val) {
        if (!val || val.length < 3) return

        this.debouncedGetProveedor()
      },
    },
    created () {
      this.debouncedGetProducto = debounce(this.getProducto, 500)
      this.debouncedGetProveedor = debounce(this.getProveedor, 500)
      this.ingreso.userId = this.userGetter.id
    },
    methods: {
      close () {
        this.$emit('close')
        this.reset()
      },
      save () {
        this.ingreso.fecha = new Date()
        this.$emit('save')
      },
      reset () {
        this.$refs.form.reset()
        this.$refs.observerValidate.reset()
      },
      customFilterProducto (item, queryText, itemText) {
        const textOne = item.nombre.toLowerCase()
        const textTwo = item.codigo.toLowerCase()
        const searchText = queryText.toLowerCase()

        return textOne.indexOf(searchText) > -1 ||
          textTwo.indexOf(searchText) > -1
      },
      async getProducto () {
        this.isLoadingProductos = true

        const response = await ProductosService.search(this.productoSearch)
        if (response.status >= 200 && response.status <= 299) {
          this.productos = response.data
        }
        this.isLoadingProductos = false
      },
      async getProveedor () {
        this.isLoadingProveedores = true

        const response = await ProveedoresService.search(this.proveedorSearch)
        if (response.status >= 200 && response.status <= 299) {
          this.proveedores = response.data
        }
        this.isLoadingProveedores = false
      },
    },
  }
</script>
