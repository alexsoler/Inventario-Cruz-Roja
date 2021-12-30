<template>
  <base-material-chart-card
    :data="productosSubscriptionChart.data"
    :options="productosSubscriptionChart.options"
    :responsive-options="productosSubscriptionChart.responsiveOptions"
    color="#E91E63"
    hover-reveal
    type="Bar"
  >
    <template v-slot:reveal-actions>
      <v-tooltip bottom>
        <template v-slot:activator="{ attrs, on }">
          <v-btn
            v-bind="attrs"
            color="info"
            icon
            v-on="on"
          >
            <v-icon color="info">
              mdi-refresh
            </v-icon>
          </v-btn>
        </template>

        <span>Refresh</span>
      </v-tooltip>

      <v-tooltip bottom>
        <template v-slot:activator="{ attrs, on }">
          <v-btn
            v-bind="attrs"
            light
            icon
            v-on="on"
          >
            <v-icon>mdi-pencil</v-icon>
          </v-btn>
        </template>

        <span>Change Date</span>
      </v-tooltip>
    </template>

    <h4
      class="card-title font-weight-light mt-2 ml-2"
      @click="obtenerResumenDeProductos"
    >
      Productos
    </h4>

    <p class="d-inline-flex font-weight-light ml-2 mt-1">
      Nuevos Productos Agregados
    </p>

    <template v-slot:actions>
      <v-icon
        class="mr-1"
        small
      >
        mdi-clock-outline
      </v-icon>
      <span
        class="text-caption grey--text font-weight-light"
      >actualizado hace {{minutos}} minutos</span>
    </template>
  </base-material-chart-card>
</template>

<script>
  import DashboardService from '@/services/dashboard.service'
  import { mapGetters } from 'vuex'

  export default {
    data: () => ({
      minutos: 0,
      intervalMinutos: null,
      productosSubscriptionChart: {
        data: {
          labels: [],
          series: [
            [],

          ],
        },
        options: {
          axisX: {
            showGrid: false,
          },
          low: 0,
          high: 10,
          chartPadding: {
            top: 0,
            right: 0,
            bottom: 0,
            left: 0,
          },
        },
        responsiveOptions: [
          ['screen and (max-width: 640px)', {
            seriesBarDistance: 5,
            axisX: {
              labelInterpolationFnc: function (value) {
                return value[0]
              },
            },
          }],
        ],
      },
    }),
    computed: {
      ...mapGetters(['mesesGetter']),
    },
    created () {
      this.obtenerResumenDeProductos()
    },
    beforeDestroy () {
      if (this.intervalMinutos) {
        clearInterval(this.intervalMinutos)
      }
    },
    methods: {
      async obtenerResumenDeProductos () {
        const response = await DashboardService.getResumenProductos()

        if (response.status >= 200 && response.status <= 299) {
          this.productosSubscriptionChart.options.high = Math.ceil(Math.max(...response.data.map(x => x.cantidad)) / 10) * 10

          this.productosSubscriptionChart.data.labels = response.data.map(x => this.mesesGetter[x.mes - 1])
          this.productosSubscriptionChart.data.series = [response.data.map(x => x.cantidad)]

          this.minutos = 0
          if (this.intervalMinutos) {
            clearInterval(this.intervalMinutos)
          }

          this.intervalMinutos = setInterval(() => {
            this.minutos++
          }, 60000)
        }

        setTimeout(async () => await this.obtenerResumenDeProductos(), 300000)
      },
    },
  }
</script>
