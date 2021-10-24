<template>
  <validation-observer
    ref="observerValidate"
    v-slot="{ invalid }"
  >
    <base-material-card>
      <template v-slot:heading>
        <div class="text-h3 font-weight-light">
          {{ modeEdit ? "Editar" : "Registrar" }} Traslado
        </div>
      </template>
      <v-form
        id="formTraslado"
        ref="form"
      >
        <validation-provider
          v-slot="{errors}"
          name="producto"
          rules="required"
        >
          <v-autocomplete
            v-model="traslado.productoId"
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
            @change="mostrarImagenProducto"
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
        <div class="d-flex flex-column justify-space-between align-center">
          <v-expand-transition>
            <v-img
              v-show="imagenProducto"
              class="mt-2"
              :aspect-ratio="4/3"
              :width="200"
              :src="imagenProducto"
            />
          </v-expand-transition>
        </div>
        <validation-provider
          v-slot="{errors}"
          name="sedeOrigen"
          rules="required"
        >
          <v-select
            v-model="traslado.sedeOrigenId"
            :items="sedesGetter"
            item-text="nombre"
            item-value="id"
            label="Sede Origen"
            prepend-icon="mdi-home-plus"
            :error-messages="errors"
          />
        </validation-provider>
        <validation-provider
          v-slot="{errors}"
          name="sedeDestino"
          rules="required|sede_traslado:@sedeOrigen"
        >
          <v-select
            v-model="traslado.sedeDestinoId"
            :items="sedesGetter"
            item-text="nombre"
            item-value="id"
            label="Sede Destino"
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
            v-model="traslado.observaciones"
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
            v-model.number="traslado.cantidad"
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
          form="formTraslado"
          :disabled="invalid || ajaxInProgress"
          :small="$vuetify.breakpoint.xsOnly"
          @click="save"
        >
          {{ modeEdit ? "Editar" : "Registrar" }}
        </v-btn>
        <v-btn
          color="error"
          class="mr-0"
          form="formTraslado"
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
      traslado: {
        type: Object,
        default: function () {
          return {
            id: 0,
            sedeOrigenId: 0,
            sedeOrigen: '',
            sedeDestinoId: 0,
            sedeDestino: '',
            productoId: 0,
            producto: '',
            userId: 0,
            userAnulaId: null,
            usuario: '',
            usuarioAnula: '',
            observaciones: '',
            motivoAnula: null,
            anulado: false,
            cantidad: 0,
            fecha: '',
            fechaAnula: '',
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
        imagenProducto: '',
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
    },
    created () {
      this.debouncedGetProducto = debounce(this.getProducto, 500)
      this.traslado.userId = this.userGetter.id
    },
    methods: {
      close () {
        this.$emit('close')
        this.reset()
      },
      save () {
        this.traslado.fecha = new Date()
        this.traslado.userId = this.userGetter.id
        this.$emit('save')
      },
      reset () {
        this.$refs.form.reset()
        this.$refs.observerValidate.reset()
      },
      mostrarImagenProducto (id) {
        const producto = this.productos.find(x => x.id === id)
        if (producto) { this.imagenProducto = producto.imagenUrl } else { this.imagenProducto = null }
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
    },
  }
</script>
