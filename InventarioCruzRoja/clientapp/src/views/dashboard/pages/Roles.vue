<template>
  <v-container
    id="roles"
    fluid
    tag="section"
  >
    <v-row justify="center">
      <v-col cols="12">
        <v-data-table
          :headers="headers"
          :items="roles"
          :search="search"
          sort-by="Id"
          class="elevation-1"
        >
          <template v-slot:top>
            <v-toolbar flat>
              <v-toolbar-title v-if="$vuetify.breakpoint.smAndUp">
                Roles
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
                    Nuevo Role
                    <v-icon dark>
                      mdi-plus
                    </v-icon>
                  </v-btn>
                </template>
                <form-role
                  :mode-edit="editedIndex !== -1"
                  :role="editedItem"
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
  import FormRole from '../component/FormRole'
  import RolesService from '@/services/roles.service'

  export default {
    components: {
      FormRole,
    },
    data: () => ({
      dialog: false,
      headers: [
        {
          text: 'Id',
          align: 'start',
          value: 'id',
        },
        { text: 'Nombre', value: 'name' },
        { text: 'Actions', value: 'actions', sortable: false },
      ],
      search: '',
      roles: [],
      editedIndex: -1,
      editedItem: {
        id: 0,
        name: '',
      },
      defaultItem: {
        id: 0,
        name: '',
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
        const response = await RolesService.getAll()
        if (response.success) {
          this.roles = response.data
        }
      },
      editItem (item) {
        this.editedIndex = this.roles.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialog = true
      },
      async deleteItem (item) {
        if (confirm('¿Esta seguro de que desea eliminar este registro?')) {
          const { success, message } = await RolesService.delete(item.id)

          if (success) {
            const index = this.roles.indexOf(item)
            this.roles.splice(index, 1)
            this.colorSnackbar = 'success'
          } else {
            this.colorSnackbar = 'error'
          }

          this.messageSnackbar = message
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
          const { success, message: messageResponse } = await RolesService.edit(this.editedItem.id, this.editedItem)
          if (success) {
            Object.assign(this.roles[this.editedIndex], this.editedItem)
            this.colorSnackbar = 'success'
            message = messageResponse
          } else {
            this.colorSnackbar = 'error'
            message = messageResponse
          }
        } else {
          const { data, success, message: messageResponse } = await RolesService.create(this.editedItem)
          if (success) {
            this.editedItem.id = data.id
            this.roles.push(this.editedItem)
            this.colorSnackbar = 'success'
          } else {
            this.colorSnackbar = 'error'
          }
          message = messageResponse
        }
        this.close()
        this.messageSnackbar = message
        this.snackbar = true
        this.isAjaxPetitionInProgress = false
      },
    },
  }
</script>
