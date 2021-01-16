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
              Reset
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
      async initialize () {
        const response = await SedesService.getAll()
        if (response.status === 200) {
          this.sedes = response.data
        }
      },
      editItem (item) {
        this.editedIndex = this.sedes.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },
      async deleteItem (item) {
        if (confirm('¿Esta seguro de que desea eliminar este registro?')) {
          const response = await SedesService.delete(item.id)

          if (response.status === 200) {
            const index = this.sedes.indexOf(item)
            this.sedes.splice(index, 1)
            this.colorSnackbar = 'success'
            this.messageSnackbar = 'Registro eliminado con exito'
          } else {
            this.colorSnackbar = 'error'
            this.messageSnackbar = response.data
          }
          this.snackbar = true
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
        let message = ''
        this.isAjaxPetitionInProgress = true
        if (this.editedIndex > -1) {
          const response = await SedesService.edit(this.editedItem.id, this.editedItem)
          if (response.status === 200) {
            Object.assign(this.sedes[this.editedIndex], this.editedItem)
            this.colorSnackbar = 'success'
            message = 'Registro editado con exito'
          } else {
            this.colorSnackbar = 'error'
            message = response.data
          }
        } else {
          const response = await SedesService.create(this.editedItem)
          if (response.status === 201) {
            this.editedItem.id = response.data.id
            this.sedes.push(this.editedItem)
            this.colorSnackbar = 'success'
            message = 'Registro creado con exito'
          } else {
            this.colorSnackbar = 'error'
            message = response.data
          }
        }
        this.close()
        this.messageSnackbar = message
        this.snackbar = true
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
