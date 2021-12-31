<template>
  <base-material-chart-card
    :data="ingresosChart.data"
    :options="ingresosChart.options"
    color="success"
    hover-reveal
    type="Line"
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

    <h4 class="card-title font-weight-light mt-2 ml-2">
      Ingresos
    </h4>

    <p class="d-inline-flex font-weight-light ml-2 mt-1">
      <v-icon
        color="green"
        small
      >
        mdi-arrow-down
      </v-icon>
      Ingresos de inventario
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
  import moment from 'moment'
  import chartist from 'chartist'

  export default {
    data: () => ({
      minutos: 0,
      intervalMinutos: null,
      ingresosChart: {
        data: {
          labels: ['M', 'T', 'W', 'T', 'F', 'S', 'S'],
          series: [
            [12, 17, 7, 17, 23, 18, 38],
          ],
        },
        options: {
          lineSmooth: chartist.Interpolation.cardinal({
            tension: 0,
          }),
          low: 0,
          high: 50,
          chartPadding: {
            top: 0,
            right: 0,
            bottom: 0,
            left: 0,
          },
        },
      },
    }),
    created () {
      moment.locale('es')
      this.obtenerResumenDeIngresos()
    },
    beforeDestroy () {
      if (this.intervalMinutos) {
        clearInterval(this.intervalMinutos)
      }
    },
    methods: {
      async obtenerResumenDeIngresos () {
        const response = await DashboardService.getResumenIngresos()

        if (response.status >= 200 && response.status <= 299) {
          this.ingresosChart.options.high = Math.ceil(Math.max(...response.data.map(x => x.cantidad)) / 10) * 10

          this.ingresosChart.data.labels = response.data.map(x => moment(x.fecha, 'YYYY-MM-DD').format('dddd').toUpperCase()[0])
          this.ingresosChart.data.series = [response.data.map(x => x.cantidad)]

          this.minutos = 0
          if (this.intervalMinutos) {
            clearInterval(this.intervalMinutos)
          }

          this.intervalMinutos = setInterval(() => {
            this.minutos++
          }, 60000)
        }

        setTimeout(async () => await this.obtenerResumenDeIngresos(), 300000)
      },
    },
  }
</script>
