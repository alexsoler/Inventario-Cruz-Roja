<template>
  <v-container
    id="proveedores"
    fluid
    tag="section"
  >
    <v-row justify="center">
      <v-col cols="12">
        <v-data-table
          :headers="headers"
          :items="proveedores"
          :search="search"
          :expanded.sync="expanded"
          :single-expand="true"
          show-expand
          sort-by="Id"
          class="elevation-1"
        >
          <template v-slot:top>
            <v-toolbar flat>
              <v-toolbar-title v-if="$vuetify.breakpoint.smAndUp">
                Proveedores
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
                max-width="700px"
              >
                <template v-slot:activator="{ on }">
                  <v-btn
                    color="primary"
                    dark
                    v-on="on"
                  >
                    Nueva Proveedor
                    <v-icon dark>
                      mdi-plus
                    </v-icon>
                  </v-btn>
                </template>
                <form-proveedor
                  :mode-edit="editedIndex !== -1"
                  :proveedor="editedItem"
                  :ajax-in-progress="isAjaxPetitionInProgress"
                  @save="save"
                  @close="close"
                />
              </v-dialog>
            </v-toolbar>
          </template>
          <template v-slot:item.telefonoFijo1="{ item }">
            <a :href="'tel:'+item.telefonoFijo1">{{ item.telefonoFijo1 }}</a>
          </template>
          <template v-slot:item.telefonoFijo2="{ item }">
            <a :href="'tel:'+item.telefonoFijo2">{{ item.telefonoFijo2 }}</a>
          </template>
          <template v-slot:item.sitioWeb="{ item }">
            <a
              :href="item.sitioWeb"
              target="_blank"
            >{{ item.sitioWeb }}</a>
          </template>
          <template v-slot:item.email="{ item }">
            <a :href="'mailto:'+item.email">{{ item.email }}</a>
          </template>
          <template v-slot:item.estadoId="{ item }">
            <v-chip
              :color="getColor(item.estadoId)"
              dark
            >
              {{ getEstado(item.estadoId) }}
            </v-chip>
          </template>
          <template v-slot:expanded-item="{ headers, item }">
            <td :colspan="headers.length">
              <v-container fluid>
                <v-data-iterator
                  :items="item.contactos"
                  :items-per-page="5"
                >
                  <template v-slot:default="props">
                    <v-row>
                      <v-col
                        v-for="item in props.items"
                        :key="item.id"
                        cols="12"
                        sm="6"
                        md="4"
                        lg="4"
                      >
                        <v-card>
                          <v-card-title class="subheading font-weight-bold">
                            {{ item.nombre }}
                          </v-card-title>

                          <v-divider />

                          <v-list dense>
                            <v-list-item>
                              <v-list-item-content>Teléfono:</v-list-item-content>
                              <v-list-item-content class="align-end">
                                <a :href="'tel:'+item.telefonoFijo1">{{ item.telefono }}</a>
                              </v-list-item-content>
                            </v-list-item>

                            <v-list-item>
                              <v-list-item-content>Correo:</v-list-item-content>
                              <v-list-item-content class="align-end">
                                <a :href="'mailto:'+item.email">{{ item.email }}</a>
                              </v-list-item-content>
                            </v-list-item>
                          </v-list>
                        </v-card>
                      </v-col>
                    </v-row>
                  </template>
                </v-data-iterator>
              </v-container>
            </td>
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
  import FormProveedor from '../component/FormProveedor'
  import ProveedoresService from '@/services/proveedores.service'

  export default {
    components: {
      FormProveedor,
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
        { text: 'Teléfono #1', value: 'telefonoFijo1' },
        { text: 'Teléfono #2', value: 'telefonoFijo2' },
        { text: 'Sitio Web', value: 'sitioWeb' },
        { text: 'Email', value: 'email' },
        { text: 'Estado', value: 'estadoId' },
        { text: 'Acción', value: 'actions', sortable: false },
        { text: '', value: 'data-table-expand' },
      ],
      expanded: [],
      search: '',
      proveedores: [],
      editedIndex: -1,
      editedItem: {
        id: 0,
        nombre: '',
        direccion: '',
        telefonoFijo1: '',
        telefonoFijo2: '',
        sitioWeb: '',
        email: null,
        contactos: [],
        estadoId: 1,
      },
      defaultItem: {
        id: 0,
        direccion: '',
        telefonoFijo1: '',
        telefonoFijo2: '',
        sitioWeb: '',
        email: null,
        contactos: [],
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
        const response = await ProveedoresService.getAll()
        if (response.status >= 200 && response.status <= 299) {
          this.proveedores = response.data
        }
      },
      editItem (item) {
        this.editedIndex = this.proveedores.indexOf(item)
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
          const response = await ProveedoresService.delete(item.id)

          if (response.status >= 200 && response.status <= 299) {
            const index = this.proveedores.indexOf(item)
            this.proveedores.splice(index, 1)
            this.$swal.fire(
              '¡Eliminado!',
              'Su registro ha sido eliminado.',
              'success',
            )
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
          const response = await ProveedoresService.edit(this.editedItem.id, this.editedItem)
          if (response.status >= 200 && response.status <= 299) {
            Object.assign(this.proveedores[this.editedIndex], this.editedItem)
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido editado.',
              'success',
            )
          } else {
            this.$swal.fire(
              '¡Error!',
              response.data,
              'error',
            )
          }
        } else {
          const response = await ProveedoresService.create(this.editedItem)
          if (response.status === 201) {
            this.editedItem.id = response.data.id
            this.proveedores.push(this.editedItem)
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido creado.',
              'success',
            )
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
