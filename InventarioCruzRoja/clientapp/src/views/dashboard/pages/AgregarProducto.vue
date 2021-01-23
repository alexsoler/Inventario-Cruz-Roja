<template>
  <v-container id="producto" fluid tag="section">
    <v-row>
      <v-col md="8">
        <v-card elevation="4">
          <base-material-card>
            <template v-slot:heading>
              <div class="display-2 font-weight-light">
                {{ modeEdit ? "Editar" : "Registrar" }} Producto
              </div>
            </template>
          </base-material-card>
        </v-card>
      </v-col>
      <v-col md="4" elevation="4"></v-col>
    </v-row>
  </v-container>
</template>

<script>
  export default {
    props: {
      modeEdit: {
        type: Boolean,
        default: false,
      },
      producto: {
        type: Object,
        default: function () {
          return {
            id: '',
            nombre: '',
            modelo: '',
            presentacion: '',
            descripcion: '',
            observaciones: '',
            fabricanteId: 0,
            sedeId: 0,
            estadoId: 0,
            costo: 0,
            stock: 0,
            imagenUrl: 0,
          }
        },
      },
    },
    data() {
      return {
      }
    }
  }
</script>

<style>
</style>