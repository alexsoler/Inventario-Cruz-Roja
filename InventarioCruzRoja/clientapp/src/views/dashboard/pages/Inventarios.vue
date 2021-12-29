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
              <v-col cols="3">
                <v-select
                  v-model="sedeSeleccionada"
                  :items="sedesGetter"
                  item-text="nombre"
                  item-value="id"
                  menu-props="auto"
                  label="Seleccionar Sede"
                  hide-details
                  prepend-icon="mdi-home-plus"
                  single-line
                  clearable
                  @change="filterChanged"
                />
              </v-col>
              <v-col cols="3">
                <v-menu
                  ref="menu"
                  v-model="menuFecha"
                  :close-on-content-click="false"
                  :nudge-right="40"
                  transition="scale-transition"
                  offset-y
                  min-width="auto"
                >
                  <template v-slot:activator="{ on, attrs }">
                    <v-text-field
                      v-model="dateRangeText"
                      label="Rango de Fechas"
                      prepend-icon="mdi-calendar"
                      readonly
                      style="margin-top: 22px"
                      clearable
                      v-bind="attrs"
                      v-on="on"
                      @click:clear="clearDate"
                    />
                  </template>
                  <v-date-picker
                    v-model="rangoFechas"
                    range
                    no-title
                    scrollable
                    @change="filterChanged"
                  >
                    <v-spacer />
                    <v-btn
                      text
                      color="primary"
                      @click="menuFecha = false"
                    >
                      Cancelar
                    </v-btn>
                    <v-btn
                      text
                      color="primary"
                      @click="$refs.menu.save(rangoFechas)"
                    >
                      OK
                    </v-btn>
                  </v-date-picker>
                </v-menu>
              </v-col>
              <v-col cols="3">
                <v-text-field
                  v-model="search"
                  append-icon="mdi-magnify"
                  label="Search"
                  single-line
                  hide-details
                />
                <v-spacer />
              </v-col>
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
          <template v-slot:item.actions="{ item }">
            <v-btn
              class="mx-2"
              fab
              dark
              x-small
              color="indigo"
              @click="mostrarTimeline(item)"
            >
              <v-icon>
                mdi-timeline-clock
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
    </v-row>
    <v-dialog
      v-model="dialogTimeline"
      scrollable
      max-width="700px"
    >
      <timeline-eventos
        :eventos="eventosProducto"
        @close="dialogTimeline = false"
      />
    </v-dialog>
  </v-container>
</template>

<script>
  import InventariosService from '@/services/inventarios.service'
  import EventosService from '@/services/eventos.service'
  import { mapGetters } from 'vuex'
  import moment from 'moment'
  import TimelineEventos from '../component/TimelineEventos.vue'

  export default {
    components: {
      TimelineEventos,
    },
    data: () => ({
      dialogTimeline: false,
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
        { text: 'Acción', value: 'actions', sortable: false },
      ],
      search: '',
      inventarios: [],
      defaultImage: require('@/assets/box.jpg'),
      isAjaxPetitionInProgress: false,
      sedeSeleccionada: null,
      menuFecha: false,
      rangoFechas: [],
      eventosProducto: [],
    }),
    computed: {
      ...mapGetters(['sedesGetter']),
      dateRangeText: {
        get () {
          return this.rangoFechas.join(' ~ ')
        },
        set (newValue) {
          return newValue
        },
      },
    },
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
      async filterChanged () {
        let fecha1 = null
        let fecha2 = null

        if (this.rangoFechas.length === 1) {
          fecha1 = moment(this.rangoFechas[0], 'YYYY-MM-DD').toDate()
        } else if (this.rangoFechas.length === 2) {
          fecha1 = moment(this.rangoFechas[0], 'YYYY-MM-DD').toDate()
          fecha2 = moment(this.rangoFechas[1], 'YYYY-MM-DD').toDate()

          if (fecha1 > fecha2) {
            fecha1 = moment(this.rangoFechas[1], 'YYYY-MM-DD').toDate()
            fecha2 = moment(this.rangoFechas[0], 'YYYY-MM-DD').toDate()
          }
        }

        const response = await InventariosService.getAll(this.sedeSeleccionada, fecha1, fecha2)
        if (response.status >= 200 && response.status <= 299) {
          this.inventarios = response.data
        }
      },
      async clearDate () {
        this.rangoFechas = []
        await this.filterChanged()
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
      async mostrarTimeline (item) {
        const response = await EventosService.getProductEvents(item.productoId)
        if (response.status >= 200 && response.status <= 299) {
          this.eventosProducto = response.data
        }
        this.dialogTimeline = true
      },
    },
  }
</script>
