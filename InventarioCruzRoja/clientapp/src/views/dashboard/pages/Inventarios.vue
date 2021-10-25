<template>
  <v-container
    id="inventarios"
    fluid
    tag="section"
  >
    <v-row justify="center">
      <v-col cols="12">
        <v-data-table
          :headers="headers"
          :items="inventarios"
          :search="search"
          sort-by="Id"
          class="elevation-1"
        >
          <template v-slot:top>
            <v-toolbar flat>
              <v-toolbar-title v-if="$vuetify.breakpoint.smAndUp">
                Inventarios
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
            </v-toolbar>
          </template>
          <template v-slot:item.imagenUrl="{ item }">
            <img
              :src="item.imagenUrl ? item.imagenUrl : defaultImage"
              alt="Producto Imagen"
              class="img-rounded"
              width="60"
            >
          </template>
          <template v-slot:item.precio="{ item }">
            {{ item.precio | currency }}
          </template>
          <template v-slot:item.stock="{ item }">
            <v-chip
              :color="getColorStock(item.stock)"
              dark
            >
              {{ item.stock }}
            </v-chip>
          </template>
          <template v-slot:item.estado="{ item }">
            <v-chip
              :color="getColorEstado(item.estado)"
              dark
            >
              {{ item.estado }}
            </v-chip>
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
    </v-row>
  </v-container>
</template>

<script>
  import InventariosService from '@/services/inventarios.service'

  export default {
    data: () => ({
      dialog: false,
      headers: [
        {
          text: 'Codigo',
          align: 'start',
          value: 'codigo',
        },
        { text: 'Imagen', value: 'imagenUrl' },
        { text: 'Producto', value: 'producto' },
        { text: 'Precio', value: 'precio' },
        { text: 'Stock', value: 'stock' },
        { text: 'Presentación', value: 'presentacion' },
        { text: 'Sede', value: 'sede' },
        { text: 'Estado', value: 'estado' },
      ],
      search: '',
      inventarios: [],
      defaultImage: require('@/assets/box.jpg'),
      isAjaxPetitionInProgress: false,
    }),
    created () {
      this.initialize()
    },
    methods: {
      async initialize () {
        const response = await InventariosService.getAll()
        if (response.status >= 200 && response.status <= 299) {
          this.inventarios = response.data
        }
      },
      getColorEstado (estado) {
        if (estado === 'Activo') return 'green'
        else if (estado === 'Inactivo') return 'red'
        else return 'black'
      },
      getColorStock (stock) {
        if (stock < 1) return 'red'
        else if (stock > 10) return 'blue'
        else return 'orange'
      },
    },
  }
</script>
