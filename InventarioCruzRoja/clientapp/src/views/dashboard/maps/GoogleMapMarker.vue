<script>
  export default {
    props: {
      google: {
        type: Object,
        required: true,
      },
      map: {
        type: Object,
        required: true,
      },
      marker: {
        type: Object,
        required: true,
      },
    },

    mounted () {
      const { Marker } = this.google.maps

      new Marker({
        position: this.marker.position,
        marker: this.marker,
        map: this.map,
        title: 'Prueba',
        icon: require('@/assets/img/marker.png'),
      })
    },

    render () { return null },
  }
</script>
