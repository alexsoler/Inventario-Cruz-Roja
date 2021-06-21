<template>
  <v-container
    id="productos"
    fluid
    tag="section"
  >
    <v-row justify="center">
      <v-col cols="12">
        <v-data-table
          :headers="headers"
          :items="productos"
          :search="search"
          sort-by="Id"
          class="elevation-1"
        >
          <template v-slot:top>
            <v-toolbar flat>
              <v-toolbar-title v-if="$vuetify.breakpoint.smAndUp">
                Productos
              </v-toolbar-title>
              <v-divider
                v-if="$vuetify.breakpoint.smAndUp"
                class="mx-4"
                inset
                vertical
              />
              <v-spacer />
              <v-text-field
                v-model="search"
                append-icon="mdi-magnify"
                label="Search"
                single-line
                hide-details
              />
              <v-spacer />
              <v-divider
                class="mx-4"
                inset
                vertical
              />
              <v-btn
                color="primary"
                dark
                :to="{ name: 'AgregarProducto' }"
              >
                Nuevo Producto
                <v-icon dark>
                  mdi-plus
                </v-icon>
              </v-btn>
            </v-toolbar>
          </template>
          <template v-slot:item.costo="{ item }">
            {{ item.costo | currency }}
          </template>
          <template v-slot:item.imagenUrl="{ item }">
            <img
              :src="item.imagenUrl"
              alt="Producto Imagen"
              class="img-rounded"
              width="60"
            >
          </template>
          <template v-slot:item.estadoId="{ item }">
            <v-chip
              :color="getColor(item.estadoId)"
              dark
            >
              {{ item.estado }}
            </v-chip>
          </template>
          <template v-slot:item.actions="{ item }">
            <v-btn
              class="mx-2"
              fab
              dark
              x-small
              color="secondary"
            >
              <v-icon>
                mdi-pencil
              </v-icon>
            </v-btn>
            <v-btn
              class="mx-2"
              fab
              dark
              x-small
              color="error"
              @click="deleteItem(item)"
            >
              <v-icon>
                mdi-delete
              </v-icon>
            </v-btn>
          </template>
          <template v-slot:no-data>
            <v-btn
              color="primary"
              @click="initialize"
            >
              Reiniciar
            </v-btn>
          </template>
        </v-data-table>
      </v-col>
      <base-material-snackbar
        v-model="snackbar"
        :type="colorSnackbar"
        v-bind="{
          top: true,
          center: true,
          timeout: 3000,
        }"
      >
        {{ messageSnackbar }}
      </base-material-snackbar>
    </v-row>
  </v-container>
</template>

<script>
  import ProductosService from '@/services/productos.service'

  export default {
    data: () => ({
      headers: [
        {
          text: 'Código',
          align: 'start',
          value: 'id',
        },
        { text: 'Imagen', value: 'imagenUrl' },
        { text: 'Modelo', value: 'modelo' },
        { text: 'Producto', value: 'nombre' },
        { text: 'Fabricante', value: 'fabricante' },
        { text: 'Estado', value: 'estadoId' },
        { text: 'Stock', value: 'stock' },
        { text: 'Precio', value: 'costo' },
        { text: 'Acción', value: 'actions', sortable: false },
      ],
      search: '',
      productos: [],
      snackbar: false,
      colorSnackbar: 'success',
      messageSnackbar: '',
    }),
    created () {
      this.initialize()
    },
    methods: {
      async initialize () {
        const response = await ProductosService.getAll()
        if (response.status >= 200 && response.status <= 299) {
          this.productos = response.data
        }
      },
      async deleteItem (item) {
        if (confirm('¿Esta seguro de que desea eliminar este registro?')) {
          const response = await ProductosService.delete(item.id)

          if (response.status >= 200 && response.status <= 299) {
            const index = this.productos.indexOf(item)
            this.productos.splice(index, 1)
            this.colorSnackbar = 'success'
            this.messageSnackbar = 'Registro eliminado con exito'
          } else {
            this.colorSnackbar = 'error'
            this.messageSnackbar = response.data
          }
          this.snackbar = true
        }
      },
      getColor (estadoId) {
        if (estadoId === 1) return 'green'
        else if (estadoId === 2) return 'red'
        else return 'black'
      },
    },
  }
</script>
