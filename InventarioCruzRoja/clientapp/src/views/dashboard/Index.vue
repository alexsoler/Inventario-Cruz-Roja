<template>
  <v-app>
    <dashboard-core-app-bar />

    <dashboard-core-drawer />

    <dashboard-core-view />

    <dashboard-core-settings />
  </v-app>
</template>

<script>
  import { mapActions } from 'vuex'

  export default {
    name: 'DashboardIndex',

    components: {
      DashboardCoreAppBar: () => import('./components/core/AppBar'),
      DashboardCoreDrawer: () => import('./components/core/Drawer'),
      DashboardCoreSettings: () => import('./components/core/Settings'),
      DashboardCoreView: () => import('./components/core/View'),
    },

    data: () => ({
      expandOnHover: false,
    }),
    async created () {
      await this.obtenerEstados()
      await this.obtenerFabricantes()
      await this.obtenerSedes()
      await this.obtenerCategorias()
    },
    methods: {
      ...mapActions(['obtenerEstados', 'obtenerFabricantes', 'obtenerSedes', 'obtenerCategorias']),
    },
  }
</script>
