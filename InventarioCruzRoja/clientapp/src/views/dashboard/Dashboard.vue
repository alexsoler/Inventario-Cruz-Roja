<template>
  <v-container
    id="dashboard"
    fluid
    tag="section"
  >
    <v-row>
      <v-col
        cols="12"
        lg="4"
      >
        <resumen-productos-chart />
      </v-col>

      <v-col
        cols="12"
        lg="4"
      >
        <resumen-ingresos-chart />
      </v-col>

      <v-col
        cols="12"
        lg="4"
      >
        <resumen-egresos-chart />
      </v-col>

      <v-col
        cols="12"
        sm="6"
        lg="3"
      >
        <base-material-stats-card
          color="info"
          icon="mdi-account"
          title="Usuarios"
          :value="usuarios"
          sub-icon="mdi-clock"
          sub-text="Recién actualizado"
        />
      </v-col>

      <v-col
        cols="12"
        sm="6"
        lg="3"
      >
        <base-material-stats-card
          color="primary"
          icon="mdi-package-variant"
          title="Productos"
          :value="productos"
          sub-icon="mdi-clock"
          sub-text="Recién actualizado"
        />
      </v-col>

      <v-col
        cols="12"
        sm="6"
        lg="3"
      >
        <base-material-stats-card
          color="indigo"
          icon="mdi-home-plus"
          title="Sedes"
          :value="sedes"
          sub-icon="mdi-clock"
          sub-text="Recién actualizado"
        />
      </v-col>

      <v-col
        cols="12"
        sm="6"
        lg="3"
      >
        <base-material-stats-card
          color="orange"
          icon="mdi-truck"
          title="Proveedores"
          :value="proveedores"
          sub-icon="mdi-clock"
          sub-text="Recién actualizado"
        />
      </v-col>

      <v-col
        cols="12"
        md="6"
      >
        <base-material-card
          color="warning"
          class="px-5 py-3"
        >
          <template v-slot:heading>
            <div class="text-h3 font-weight-light">
              Eventos
            </div>

            <div class="text-subtitle-1 font-weight-light">
              Ultimos eventos respecto al inventario
            </div>
          </template>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="ultimosEventos"
            >
              <template v-slot:item.fecha="{ item }">
                <span>{{ new Date(item.fecha).toLocaleString() }}</span>
              </template>
            </v-data-table>
          </v-card-text>
        </base-material-card>
      </v-col>

      <v-col
        cols="12"
        md="6"
      >
        <base-material-card class="px-5 py-3">
          <template v-slot:heading>
            <div class="text-h3 font-weight-light">
              Chat
            </div>

            <div class="text-subtitle-1 font-weight-light">
              Mensajes de usuarios
            </div>
          </template>
          <v-card-text>
            <chat />
          </v-card-text>
        </base-material-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import ResumenProductosChart from '../dashboard/component/ResumenProductosChart.vue'
  import ResumenIngresosChart from '../dashboard/component/ResumenIngresosChart.vue'
  import ResumenEgresosChart from '../dashboard/component/ResumenEgresosChart.vue'
  import Chat from '../dashboard/component/Chat.vue'
  import DashboardService from '@/services/dashboard.service'

  export default {
    name: 'DashboardDashboard',
    components: {
      ResumenProductosChart,
      ResumenIngresosChart,
      ResumenEgresosChart,
      Chat,
    },
    data () {
      return {
        usuarios: '0',
        sedes: '0',
        productos: '0',
        proveedores: '0',
        ultimosEventos: [],
        headers: [
          {
            sortable: false,
            text: 'Descripcion',
            value: 'descripcion',
          },
          {
            sortable: false,
            text: 'Fecha',
            value: 'fecha',
          },
        ],
        items: [
          {
            id: 1,
            name: 'Dakota Rice',
            country: 'Niger',
            city: 'Oud-Tunrhout',
            salary: '$35,738',
          },
          {
            id: 2,
            name: 'Minerva Hooper',
            country: 'Curaçao',
            city: 'Sinaai-Waas',
            salary: '$23,738',
          },
          {
            id: 3,
            name: 'Sage Rodriguez',
            country: 'Netherlands',
            city: 'Overland Park',
            salary: '$56,142',
          },
          {
            id: 4,
            name: 'Philip Chanley',
            country: 'Korea, South',
            city: 'Gloucester',
            salary: '$38,735',
          },
          {
            id: 5,
            name: 'Doris Greene',
            country: 'Malawi',
            city: 'Feldkirchen in Kārnten',
            salary: '$63,542',
          },
        ],
        tabs: 0,
        tasks: {
          0: [
            {
              text: 'Sign contract for "What are conference organizers afraid of?"',
              value: true,
            },
            {
              text: 'Lines From Great Russian Literature? Or E-mails From My Boss?',
              value: false,
            },
            {
              text: 'Flooded: One year later, assessing what was lost and what was found when a ravaging rain swept through metro Detroit',
              value: false,
            },
            {
              text: 'Create 4 Invisible User Experiences you Never Knew About',
              value: true,
            },
          ],
          1: [
            {
              text: 'Flooded: One year later, assessing what was lost and what was found when a ravaging rain swept through metro Detroit',
              value: true,
            },
            {
              text: 'Sign contract for "What are conference organizers afraid of?"',
              value: false,
            },
          ],
          2: [
            {
              text: 'Lines From Great Russian Literature? Or E-mails From My Boss?',
              value: false,
            },
            {
              text: 'Flooded: One year later, assessing what was lost and what was found when a ravaging rain swept through metro Detroit',
              value: true,
            },
            {
              text: 'Sign contract for "What are conference organizers afraid of?"',
              value: true,
            },
          ],
        },
        list: {
          0: false,
          1: false,
          2: false,
        },
      }
    },
    created () {
      this.obtenerCantidadDeUsuarios()
      this.obtenerCantidadDeSedes()
      this.obtenerCantidadDeProductos()
      this.obtenerCantidadDeProveedores()
      this.obtenerUltimosEventos()
    },
    methods: {
      complete (index) {
        this.list[index] = !this.list[index]
      },
      async obtenerCantidadDeUsuarios () {
        const response = await DashboardService.getCantidadDeUsuarios()
        if (response.status >= 200 && response.status <= 299) {
          this.usuarios = response.data.toString()
        }
      },
      async obtenerCantidadDeSedes () {
        const response = await DashboardService.getCantidadDeSedes()
        if (response.status >= 200 && response.status <= 299) {
          this.sedes = response.data.toString()
        }
      },
      async obtenerCantidadDeProductos () {
        const response = await DashboardService.getCantidadDeProductos()
        if (response.status >= 200 && response.status <= 299) {
          this.productos = response.data.toString()
        }
      },
      async obtenerCantidadDeProveedores () {
        const response = await DashboardService.getCantidadDeProveedores()
        if (response.status >= 200 && response.status <= 299) {
          this.proveedores = response.data.toString()
        }
      },
      async obtenerUltimosEventos () {
        const response = await DashboardService.getUltimosEventos()
        if (response.status >= 200 && response.status <= 299) {
          this.ultimosEventos = response.data
        }
      },
    },
  }
</script>
