<template>
  <v-data-table
    :headers="headers"
    :items="contactosLocal"
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
          <validation-observer
            ref="observerValidateContactos"
            v-slot="{ invalid }"
            :disabled="!dialog || !dialogDelete"
          >
            <v-card>
              <v-card-title>
                <span class="text-h5">{{ formTitle }}</span>
              </v-card-title>

              <v-card-text>
                <v-container>
                  <v-row>
                    <v-col
                      cols="12"
                    >
                      <validation-provider
                        v-slot="{errors}"
                        name="nombre"
                        rules="required|min:2|max:50"
                      >
                        <v-text-field
                          v-model="editedItem.nombre"
                          label="Nombre"
                          :error-messages="errors"
                        />
                      </validation-provider>
                    </v-col>
                    <v-col
                      cols="12"
                      sm="6"
                    >
                      <vue-tel-input-vuetify
                        v-model="editedItem.telefono"
                        label="Teléfono"
                        name="telefono"
                        placeholder="Teléfono"
                      />
                    </v-col>
                    <v-col
                      cols="12"
                      sm="6"
                    >
                      <validation-provider
                        v-slot="{errors}"
                        name="email"
                        rules="email|max:100"
                      >
                        <v-text-field
                          v-model="editedItem.email"
                          label="Email"
                          :error-messages="errors"
                        />
                      </validation-provider>
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
                  :disabled="invalid"
                  @click="save"
                >
                  Guardar
                </v-btn>
              </v-card-actions>
            </v-card>
          </validation-observer>
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
    <template v-slot:item.telefono="{ item }">
      <a :href="'tel:'+item.telefono">{{ item.telefono }}</a>
    </template>
    <template v-slot:item.email="{ item }">
      <a :href="'mailto:'+item.mail">{{ item.email }}</a>
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
  </v-data-table>
</template>

<script>
  import ContactosService from '@/services/contactos.service'

  export default {
    model: {
      prop: 'contactos',
      event: 'contactoschange',
    },
    props: {
      contactos: {
        type: Array,
        default: function () {
          return []
        },
      },
      proveedorId: {
        type: Number,
        default: 0,
      },
    },
    data: () => ({
      dialog: false,
      dialogDelete: false,
      headers: [
        { text: 'Nombre', value: 'nombre' },
        { text: 'Teléfono', value: 'telefono' },
        { text: 'Email', value: 'email' },
        { text: 'Acción', value: 'actions', sortable: false },
      ],
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
      contactosLocal: {
        get: function () {
          return this.contactos
        },
        set: function (value) {
          this.$emit('contactoschange', value)
        },
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

    methods: {
      editItem (item) {
        this.editedIndex = this.contactosLocal.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },

      deleteItem (item) {
        this.editedIndex = this.contactosLocal.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialogDelete = true
      },

      async deleteItemConfirm () {
        if (this.editedItem.proveedorId) {
          const response = await ContactosService.delete(this.editedItem.id)

          if (response.status >= 200 && response.status <= 299) {
            this.contactosLocal.splice(this.editedIndex, 1)
            this.closeDelete()
            this.$swal.fire(
              '¡Eliminado!',
              'Su registro ha sido eliminado.',
              'success',
            )
          } else {
            this.$swal.fire(
              '¡Error!',
              response.response.data,
              'error',
            )
          }
        } else {
          this.contactosLocal.splice(this.editedIndex, 1)
          this.closeDelete()
        }
      },

      close () {
        this.dialog = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
          this.$refs.observerValidateContactos.reset()
        })
      },

      closeDelete () {
        this.dialogDelete = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
          this.$refs.observerValidateContactos.reset()
        })
      },

      async save () {
        if (this.editedIndex > -1) {
          if (this.editedItem.proveedorId) {
            const response = await ContactosService.edit(this.editedItem.id, this.editedItem)
            if (response.status >= 200 && response.status <= 299) {
              Object.assign(this.contactosLocal[this.editedIndex], this.editedItem)
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
            Object.assign(this.contactosLocal[this.editedIndex], this.editedItem)
          }
        } else {
          if (this.proveedorId) {
            this.editedItem.proveedorId = this.proveedorId
            const response = await ContactosService.create(this.editedItem)
            if (response.status === 201) {
              this.editedItem.id = response.data.id
              this.contactosLocal.push(this.editedItem)
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
          } else {
            this.contactosLocal.push(this.editedItem)
          }
        }
        this.close()
      },
    },
  }
</script>
