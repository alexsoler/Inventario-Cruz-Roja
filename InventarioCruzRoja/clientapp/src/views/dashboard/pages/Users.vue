<template>
  <v-container
    id="users"
    fluid
    tag="section"
  >
    <v-row justify="center">
      <v-col cols="12">
        <v-data-table
          :headers="headers"
          :items="users"
          :search="search"
          sort-by="Id"
          class="elevation-1"
        >
          <template v-slot:top>
            <v-toolbar flat>
              <v-toolbar-title
                v-if="$vuetify.breakpoint.smAndUp"
              >
                Usuarios
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
                    Nuevo Usuario
                    <v-icon dark>
                      mdi-plus
                    </v-icon>
                  </v-btn>
                </template>
                <form-user
                  :mode-edit="editedIndex !== -1"
                  :user="editedItem"
                  :status-dialog="dialog"
                  :ajax-in-progress="isAjaxPetitionInProgress"
                  @save="save"
                  @close="close"
                />
              </v-dialog>
            </v-toolbar>
          </template>
          <template v-slot:item.roles="{ item }">
            <v-chip
              v-for="(role, i) in item.roles"
              :key="i"
              dark
              class="mr-1"
            >
              {{ role }}
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
  import FormUser from '../component/FormUser'
  import AuthService from '@/services/auth.service'

  export default {
    components: {
      FormUser,
    },
    data: () => ({
      dialog: false,
      headers: [
        {
          text: 'Id',
          align: 'start',
          value: 'id',
        },
        { text: 'Nombre de Usuario', value: 'username' },
        { text: 'Roles', value: 'roles' },
        { text: 'Actions', value: 'actions', sortable: false },
      ],
      search: '',
      users: [],
      editedIndex: -1,
      editedItem: {
        id: 0,
        username: '',
        roles: [],
      },
      defaultItem: {
        id: 0,
        username: '',
        roles: [],
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
        const response = await AuthService.getAllUsers()
        if (response.success) {
          this.users = response.data
        }
      },
      editItem (item) {
        this.editedIndex = this.users.indexOf(item)
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
          const { success, message } = await AuthService.delete(item.id)

          if (success) {
            const index = this.users.indexOf(item)
            this.users.splice(index, 1)
            this.$swal.fire(
              '¡Eliminado!',
              'Su registro ha sido eliminado.',
              'success',
            )
          } else {
            this.$swal.fire(
              '¡Error!',
              message,
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
      async save (password) {
        this.isAjaxPetitionInProgress = true
        if (this.editedIndex > -1) {
          if (await AuthService.edit(this.editedItem.id, { ...this.editedItem, password })) {
            Object.assign(this.users[this.editedIndex], this.editedItem)
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido editado.',
              'success',
            )
          } else {
            this.$swal.fire(
              '¡Error!',
              'No se pudo editar el usuario.',
              'error',
            )
          }
        } else {
          const { data, success, message: messageResponse } = await AuthService.register({ ...this.editedItem, password })
          if (success) {
            this.editedItem.id = data
            this.users.push({ ...this.editedItem })
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido creado.',
              'success',
            )
          } else {
            this.$swal.fire(
              '¡Error!',
              messageResponse,
              'error',
            )
          }
        }
        this.close()
        this.isAjaxPetitionInProgress = false
      },
    },
  }
</script>

<style lang="sass" scoped>

</style>
