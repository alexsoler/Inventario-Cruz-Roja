<template>
  <v-container
    id="producto"
    fluid
    tag="section"
  >
    <v-row>
      <v-col md="8">
        <form-producto
          ref="formProducto"
          :mode-edit="isModeEdit"
          :producto="producto"
          @onSave="guardarCambios"
        />
      </v-col>
      <v-col
        class="elevation"
        md="4"
      >
        <v-card elevation="4">
          <v-img
            height="250"
            :src="imageUrl"
          />

          <v-card-actions>
            <v-file-input
              v-model="image"
              :rules="rules"
              accept="image/png, image/jpeg, image/bmp"
              placeholder="Elige una imagen"
              prepend-icon="mdi-camera"
              label="Imagen"
              @change="previewImage"
            />
          </v-card-actions>
        </v-card>
        <v-card v-if="producto.codigo">
          <div class="text-center">
            <vue-barcode
              v-model="producto.codigo"
            />
          </div>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import FormProducto from '../component/FormProducto'
  import ProductosService from '@/services/productos.service'
  import VueBarcode from 'vue-barcode'

  export default {
    components: {
      FormProducto,
      VueBarcode,
    },
    props: {
      id: {
        type: Number,
        default: 0,
      },
    },
    data () {
      return {
        producto: {
          id: 0,
          codigo: '',
          nombre: '',
          modelo: '',
          presentacion: '',
          descripcion: '',
          observaciones: '',
          fabricanteId: 0,
          categoriaId: 0,
          sedes: [],
          estadoId: 0,
          costo: 0,
          imagenUrl: '',
        },
        isModeEdit: false,
        rules: [
          value => !value || value.size < 2000000 || '¡El tamaño de la imagen debe ser inferior a 2 MB!',
        ],
        imageUrl: require('@/assets/box.jpg'),
        image: null,
      }
    },
    watch: {
      image (newImage, oldImage) {
        this.producto.imagenUrl = `/Resources/Images/${newImage.name}`
      },
    },
    beforeRouteEnter (to, from, next) {
      if (to.params.id) {
        next(vm => {
          vm.obtenerProducto(to.params.id).then(x => {
            vm.isModeEdit = true
          })
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
        const response = await ProductosService.get(id)

        if (response.status >= 200 && response.status <= 299) {
          this.producto = response.data
          if (response.data.imagenUrl) {
            this.imageUrl = response.data.imagenUrl
          }
        }
      },
      previewImage () {
        if (this.image) {
          this.imageUrl = URL.createObjectURL(this.image)
        } else {
          this.imageUrl = require('@/assets/box.jpg')
        }
      },
      async guardarCambios (data) {
        if (this.isModeEdit) {
          const response = await ProductosService.edit(this.producto.id, this.producto, this.image)

          if (response.status >= 200 && response.status <= 299) {
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido editado.',
              'success',
            )

            this.$router.push({ name: 'Productos' })
          }
        } else {
          const response = await ProductosService.create(data, this.image)

          if (response.status >= 200 && response.status <= 299) {
            const result = await this.$swal.fire({
              title: '¡Exito!',
              text: 'Su registro ha sido creado ¿Desea crear otro?',
              icon: 'success',
              showCancelButton: true,
              confirmButtonColor: '#3085d6',
              cancelButtonColor: '#d33',
              confirmButtonText: 'Sí',
              cancelButtonText: 'No',
            })

            if (result.isConfirmed) {
              this.$refs.formProducto.reset()
              this.imageUrl = require('@/assets/box.jpg')
              this.image = null
            } else {
              this.$router.push({ name: 'Productos' })
            }
          } else {
            this.$swal.fire(
              '¡Error!',
              response.response.data,
              'error',
            )
          }
        }
      },
    },
  }
</script>

<style>
</style>
