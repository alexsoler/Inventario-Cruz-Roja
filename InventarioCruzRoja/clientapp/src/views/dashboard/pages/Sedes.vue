<template>
  <v-container
    id="sedes"
    fluid
    tag="section"
  >
    <v-row justify="center">
      <v-col cols="12">
        <v-data-table
          :headers="headers"
          :items="sedes"
          :search="search"
          sort-by="Id"
          class="elevation-1"
        >
          <template v-slot:top>
            <v-toolbar flat>
              <v-toolbar-title v-if="$vuetify.breakpoint.smAndUp">
                Sedes
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
                    Nueva Sede
                    <v-icon dark>
                      mdi-plus
                    </v-icon>
                  </v-btn>
                </template>
                <form-sede
                  :mode-edit="editedIndex !== -1"
                  :sede="editedItem"
                  :ajax-in-progress="isAjaxPetitionInProgress"
                  @save="save"
                  @close="close"
                />
              </v-dialog>
            </v-toolbar>
          </template>
          <template v-slot:item.estadoId="{ item }">
            <v-chip
              :color="getColor(item.estadoId)"
              dark
            >
              {{ getEstado(item.estadoId) }}
            </v-chip>
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
  import FormSede from '../component/FormSede'
  import SedesService from '@/services/sedes.service'
  import { mapActions } from 'vuex'

  export default {
    components: {
      FormSede,
    },
    data: () => ({
      dialog: false,
      headers: [
        {
          text: 'Id',
          align: 'start',
          value: 'id',
        },
        { text: 'Nombre', value: 'nombre' },
        { text: 'Dirección', value: 'direccion' },
        { text: 'Estado', value: 'estadoId' },
        { text: 'Acción', value: 'actions', sortable: false },
      ],
      search: '',
      sedes: [],
      editedIndex: -1,
      editedItem: {
        id: 0,
        nombre: '',
        direccion: '',
        estadoId: 1,
      },
      defaultItem: {
        id: 0,
        nombre: '',
        direccion: '',
        estadoId: 1,
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
      ...mapActions(['obtenerSedes']),
      async initialize () {
        const response = await SedesService.getAll()
        if (response.status >= 200 && response.status <= 299) {
          this.sedes = response.data
        }
      },
      editItem (item) {
        this.editedIndex = this.sedes.indexOf(item)
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
          const response = await SedesService.delete(item.id)

          if (response.status >= 200 && response.status <= 299) {
            const index = this.sedes.indexOf(item)
            this.sedes.splice(index, 1)
            this.$swal.fire(
              '¡Eliminado!',
              'Su registro ha sido eliminado.',
              'success',
            )
            this.obtenerSedes()
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
          const response = await SedesService.edit(this.editedItem.id, this.editedItem)
          if (response.status >= 200 && response.status <= 299) {
            Object.assign(this.sedes[this.editedIndex], this.editedItem)
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido editado.',
              'success',
            )
            this.obtenerSedes()
          } else {
            this.$swal.fire(
              '¡Error!',
              response.data,
              'error',
            )
          }
        } else {
          const response = await SedesService.create(this.editedItem)
          if (response.status === 201) {
            this.editedItem.id = response.data.id
            this.sedes.push(this.editedItem)
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido creado.',
              'success',
            )
            this.obtenerSedes()
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
