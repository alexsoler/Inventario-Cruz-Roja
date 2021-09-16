<template>
  <v-container
    id="ingresos"
    fluid
    tag="section"
  >
    <v-row justify="center">
      <v-col cols="12">
        <v-data-table
          :headers="headers"
          :items="ingresos"
          :search="search"
          sort-by="Id"
          class="elevation-1"
        >
          <template v-slot:top>
            <v-toolbar flat>
              <v-toolbar-title v-if="$vuetify.breakpoint.smAndUp">
                Ingresos
              </v-toolbar-title>
              <v-divider
                v-if="$vuetify.breakpoint.smAndUp"
                class="mx-4"
                inset
                vertical
              />
              <v-spacer />
              <v-text-field
                v-model="search"
                append-icon="mdi-magnify"
                label="Search"
                single-line
                hide-details
              />
              <v-spacer />
              <v-divider
                class="mx-4"
                inset
                vertical
              />
              <v-dialog
                v-model="dialog"
                max-width="500px"
              >
                <template v-slot:activator="{ on }">
                  <v-btn
                    color="primary"
                    dark
                    v-on="on"
                  >
                    Nuevo Ingreso
                    <v-icon dark>
                      mdi-plus
                    </v-icon>
                  </v-btn>
                </template>
                <form-ingreso
                  :mode-edit="editedIndex !== -1"
                  :ingreso="editedItem"
                  :ajax-in-progress="isAjaxPetitionInProgress"
                  @save="save"
                  @close="close"
                />
              </v-dialog>
            </v-toolbar>
          </template>
          <template v-slot:item.actions="{ item }">
            <v-btn
              class="mx-2"
              fab
              dark
              x-small
              color="secondary"
              @click="editItem(item)"
            >
              <v-icon>
                mdi-pencil
              </v-icon>
            </v-btn>
            <v-btn
              class="mx-2"
              fab
              dark
              x-small
              color="error"
              @click="deleteItem(item)"
            >
              <v-icon>
                mdi-delete
              </v-icon>
            </v-btn>
          </template>
          <template v-slot:no-data>
            <v-btn
              color="primary"
              @click="initialize"
            >
              Reiniciar
            </v-btn>
          </template>
        </v-data-table>
      </v-col>
      <base-material-snackbar
        v-model="snackbar"
        :type="colorSnackbar"
        v-bind="{
          top: true,
          center: true,
          timeout: 3000,
        }"
      >
        {{ messageSnackbar }}
      </base-material-snackbar>
    </v-row>
  </v-container>
</template>

<script>
  import FormIngreso from '../component/FormIngreso'
  import IngresosService from '@/services/ingresos.service'

  export default {
    components: {
      FormIngreso,
    },
    data: () => ({
      dialog: false,
      headers: [
        {
          text: 'Id',
          align: 'start',
          value: 'id',
        },
        { text: 'Cantidad', value: 'cantidad' },
        { text: 'Producto', value: 'producto' },
        { text: 'Proveedor', value: 'proveedor' },
        { text: 'Sede', value: 'sede' },
        { text: 'Usuario', value: 'usuario' },
        { text: 'Observaciones', value: 'observaciones' },
        { text: 'Fecha', value: 'fecha' },
        { text: 'Anulado', value: 'anulado' },
        { text: 'Acción', value: 'actions', sortable: false },
      ],
      search: '',
      ingresos: [],
      editedIndex: -1,
      editedItem: {
        id: 0,
        productoId: 0,
        producto: '',
        proveedorId: 0,
        proveedor: '',
        sedeId: 0,
        sede: '',
        userId: 0,
        usuario: '',
        observaciones: '',
        anulado: false,
        cantidad: 0,
        fecha: '',
      },
      defaultItem: {
        id: 0,
        productoId: 0,
        producto: '',
        proveedorId: 0,
        proveedor: '',
        sedeId: 0,
        sede: '',
        userId: 0,
        usuario: '',
        observaciones: '',
        anulado: false,
        cantidad: 0,
        fecha: '',
      },
      snackbar: false,
      colorSnackbar: 'success',
      messageSnackbar: '',
      isAjaxPetitionInProgress: false,
    }),
    watch: {
      dialog (val) {
        val || this.close()
      },
    },
    created () {
      this.initialize()
    },
    methods: {
      async initialize () {
        const response = await IngresosService.getAll()
        if (response.status >= 200 && response.status <= 299) {
          this.ingresos = response.data
        }
      },
      editItem (item) {
        this.editedIndex = this.ingresos.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },
      async deleteItem (item) {
        const result = await this.$swal({
          title: '¿Esta seguro de que desea eliminar este registro?',
          text: '¡No podrás revertir esto!',
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#3085d6',
          cancelButtonColor: '#d33',
          confirmButtonText: 'Sí',
          cancelButtonText: 'No',
        })

        if (result.isConfirmed) {
          const response = await IngresosService.delete(item.id)

          if (response.status >= 200 && response.status <= 299) {
            const index = this.ingresos.indexOf(item)
            this.ingresos.splice(index, 1)
            this.$swal.fire(
              '¡Eliminado!',
              'Su registro ha sido eliminado.',
              'success',
            )
            this.obtenerIngresos()
          } else {
            this.$swal.fire(
              '¡Error!',
              response.data,
              'error',
            )
          }
        }
      },
      close () {
        this.dialog = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },
      async save () {
        this.isAjaxPetitionInProgress = true
        if (this.editedIndex > -1) {
          const response = await IngresosService.edit(this.editedItem.id, this.editedItem)
          if (response.status >= 200 && response.status <= 299) {
            Object.assign(this.ingresos[this.editedIndex], this.editedItem)
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido editado.',
              'success',
            )
            this.obtenerIngresos()
          } else {
            this.$swal.fire(
              '¡Error!',
              response.data,
              'error',
            )
          }
        } else {
          const response = await IngresosService.create(this.editedItem)
          if (response.status === 201) {
            this.editedItem.id = response.data.id
            this.ingresos.push(this.editedItem)
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido creado.',
              'success',
            )
            this.obtenerIngresos()
          } else {
            this.$swal.fire(
              '¡Error!',
              response.data,
              'error',
            )
          }
        }
        this.close()
        this.isAjaxPetitionInProgress = false
      },
      getColor (estadoId) {
        if (estadoId === 1) return 'green'
        else if (estadoId === 2) return 'red'
        else return 'black'
      },
      getEstado (estadoId) {
        if (estadoId === 1) return 'Activo'
        else if (estadoId === 2) return 'Inactivo'
        else return 'Desconocido'
      },
    },
  }
</script>
