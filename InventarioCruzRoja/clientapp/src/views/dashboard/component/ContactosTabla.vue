<template>
  <v-data-table
    :headers="headers"
    :items="contactos"
    sort-by="Nombre"
    class="elevation-1"
    dense
    disable-pagination
    hide-default-footer
  >
    <template v-slot:top>
      <v-toolbar
        flat
        dense
      >
        <v-toolbar-title>Contactos</v-toolbar-title>
        <v-divider
          class="mx-4"
          inset
          vertical
        />
        <v-spacer />
        <v-dialog
          v-model="dialog"
          max-width="500px"
        >
          <template v-slot:activator="{ on, attrs }">
            <v-btn
              color="secondary"
              dark
              small
              class="mb-2"
              v-bind="attrs"
              v-on="on"
            >
              Nuevo
            </v-btn>
          </template>
          <v-card>
            <v-card-title>
              <span class="text-h5">{{ formTitle }}</span>
            </v-card-title>

            <v-card-text>
              <v-container>
                <v-row>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.nombre"
                      label="Nombre"
                    />
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.telefono"
                      label="Telefono"
                    />
                  </v-col>
                  <v-col
                    cols="12"
                    sm="6"
                    md="4"
                  >
                    <v-text-field
                      v-model="editedItem.email"
                      label="Email"
                    />
                  </v-col>
                </v-row>
              </v-container>
            </v-card-text>

            <v-card-actions>
              <v-spacer />
              <v-btn
                color="blue darken-1"
                text
                @click="close"
              >
                Cancelar
              </v-btn>
              <v-btn
                color="blue darken-1"
                text
                @click="save"
              >
                Guardar
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog
          v-model="dialogDelete"
          max-width="500px"
        >
          <v-card>
            <v-card-title class="text-h5">
              ¿Está seguro de que desea eliminar este elemento?
            </v-card-title>
            <v-card-actions>
              <v-spacer />
              <v-btn
                color="blue darken-1"
                text
                @click="closeDelete"
              >
                Cancelar
              </v-btn>
              <v-btn
                color="blue darken-1"
                text
                @click="deleteItemConfirm"
              >
                OK
              </v-btn>
              <v-spacer />
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
    </template>
    <template v-slot:item.actions="{ item }">
      <v-icon
        small
        class="mr-2"
        @click="editItem(item)"
      >
        mdi-pencil
      </v-icon>
      <v-icon
        small
        @click="deleteItem(item)"
      >
        mdi-delete
      </v-icon>
    </template>
    <template v-slot:no-data>
      <v-btn
        small
        color="primary"
        @click="initialize"
      >
        Restablecer
      </v-btn>
    </template>
  </v-data-table>
</template>

<script>
  import ContactosService from '@/services/contactos.service'

  export default {
    data: () => ({
      dialog: false,
      dialogDelete: false,
      headers: [
        { text: 'Nombre', value: 'nombre' },
        { text: 'Teléfono', value: 'telefono' },
        { text: 'Email', value: 'email' },
        { text: 'Acción', value: 'actions', sortable: false },
      ],
      contactos: [],
      editedIndex: -1,
      editedItem: {
        id: 0,
        nombre: '',
        telefono: '',
        email: '',
        proveedorId: 0,
      },
      defaultItem: {
        id: 0,
        nombre: '',
        telefono: '',
        email: '',
        proveedorId: 0,
      },
    }),

    computed: {
      formTitle () {
        return this.editedIndex === -1 ? 'Nuevo' : 'Editar'
      },
    },

    watch: {
      dialog (val) {
        val || this.close()
      },
      dialogDelete (val) {
        val || this.closeDelete()
      },
    },

    created () {
      this.initialize()
    },

    methods: {
      async initialize () {
        const response = await ContactosService.getAll()
        if (response.status >= 200 && response.status <= 299) {
          this.contactos = response.data
        }
      },

      editItem (item) {
        this.editedIndex = this.contactos.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },

      deleteItem (item) {
        this.editedIndex = this.contactos.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialogDelete = true
      },

      deleteItemConfirm () {
        this.contactos.splice(this.editedIndex, 1)
        this.closeDelete()
      },

      close () {
        this.dialog = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },

      closeDelete () {
        this.dialogDelete = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },

      save () {
        if (this.editedIndex > -1) {
          Object.assign(this.contactos[this.editedIndex], this.editedItem)
        } else {
          this.contactos.push(this.editedItem)
        }
        this.close()
      },
    },
  }
</script>
