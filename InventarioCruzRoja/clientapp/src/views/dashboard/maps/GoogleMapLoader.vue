<template>
  <div>
    <div
      ref="googleMap"
      class="google-map"
    />
    <template v-if="Boolean(google) && Boolean(map)">
      <slot
        :google="google"
        :map="map"
      />
    </template>
  </div>
</template>

<script>
  import GoogleMapsApiLoader from 'google-maps-api-loader'

  export default {
    props: {
      mapConfig: {
        type: Object,
        default: null,
      },
      apiKey: {
        type: String,
        default: '',
      },
    },

    data () {
      return {
        google: null,
        map: null,
      }
    },

    watch: {
      'mapConfig.center' (val) {
        this.map.setZoom(15)
        this.map.setCenter(new this.google.maps.LatLng(val.lat, val.lng))
      },
    },

    async mounted () {
      const googleMapApi = await GoogleMapsApiLoader({
        apiKey: this.apiKey,
      })
      this.google = googleMapApi
      this.initializeMap()
    },

    methods: {
      initializeMap () {
        const mapContainer = this.$refs.googleMap
        this.map = new this.google.maps.Map(
          mapContainer, this.mapConfig,
        )
      },
    },
  }
</script>

<style scoped>
.google-map {
  width: 100%;
  min-height: 100%;
}
</style>
