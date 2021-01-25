<template>
  <v-container
    id="producto"
    fluid
    tag="section"
  >
    <v-row>
      <v-col md="8">
        <form-producto
          :mode-edit="isModeEdit"
          :producto="producto"
        />
      </v-col>
      <v-col
        class="elevation"
        md="4"
      >
        <v-card elevation="4">
          <v-list-item three-line>
            <v-list-item-content>
              <div class="overline mb-4">
                OVERLINE
              </div>
              <v-list-item-title class="headline mb-1">
                Headline 5
              </v-list-item-title>
              <v-list-item-subtitle>Greyhound divisely hello coldly fonwderfully</v-list-item-subtitle>
            </v-list-item-content>

            <v-list-item-avatar
              tile
              size="80"
              color="grey"
            />
          </v-list-item>

          <v-card-actions>
            <v-btn
              outlined
              rounded
              text
            >
              Button
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import FormProducto from '../component/FormProducto'
  import ProductosService from '@/services/productos.service'

  export default {
    components: {
      FormProducto,
    },
    props: {
      id: {
        type: String,
        default: '',
      },
    },
    data () {
      return {
        producto: null,
        isModeEdit: false,
      }
    },
    beforeRouteEnter (to, from, next) {
      if (to.params.id) {
        this.obtenerProducto(to.params.id).then(x => {
          this.isModeEdit = true
          next()
        })
      } else {
        next()
      }
    },
    beforeRouteUpdate (to, from, next) {
      this.producto = null
      this.isModeEdit = false

      if (to.params.id) {
        this.obtenerProducto(to.params.id).then(x => {
          this.isModeEdit = true
          next()
        })
      } else {
        this.isModeEdit = false
        next()
      }
    },
    methods: {
      async obtenerProducto (id) {
        var response = await ProductosService.get(id)

        if (response.status === 200) { this.producto = response.data }
      },
    },
  }
</script>

<style>
</style>
