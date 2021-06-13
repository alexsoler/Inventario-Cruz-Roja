<template>
  <router-view />
</template>

<script>
  import { mapActions } from 'vuex'

  export default {
    name: 'App',
    async created () {
      await this.obtenerEstados()
      await this.obtenerFabricantes()
      await this.obtenerSedes()
    },
    methods: {
      ...mapActions(['obtenerEstados', 'obtenerFabricantes', 'obtenerSedes']),
    },
  }
</script>
