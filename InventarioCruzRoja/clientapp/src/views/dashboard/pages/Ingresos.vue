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
          sort-by="id"
          :expanded.sync="expanded"
          :single-expand="true"
          item-key="id"
          show-expand
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
              <v-dialog
                v-model="dialogAnulacion"
                max-width="500px"
              >
                <validation-observer
                  ref="observerValidateAnulacion"
                  v-slot="{ invalid }"
                >
                  <v-card>
                    <v-card-title class="text-h5">
                      ¿Está seguro de que desea anular este ingreso?
                    </v-card-title>
                    <v-spacer />
                    <v-card-text>
                      <v-form
                        id="formIngreso"
                        ref="formIngreso"
                      >
                        <validation-provider
                          v-slot="{errors}"
                          name="motivaAnula"
                          rules="required|min:10|max:300"
                        >
                          <v-textarea
                            v-model="editedItem.motivoAnula"
                            filled
                            label="Motivo de Anulación"
                            name="motivaAnula"
                            prepend-icon="mdi-comment-text"
                            auto-grow
                            :error-messages="errors"
                          />
                        </validation-provider>
                      </v-form>
                    </v-card-text>
                    <v-card-actions>
                      <v-spacer />
                      <v-btn
                        color="blue darken-1"
                        text
                        @click="closeAnulacion"
                      >
                        Cancelar
                      </v-btn>
                      <v-btn
                        color="blue darken-1"
                        text
                        :disabled="invalid || isAjaxPetitionInProgress"
                        @click="anulacionItemConfirm"
                      >
                        OK
                      </v-btn>
                      <v-spacer />
                    </v-card-actions>
                  </v-card>
                </validation-observer>
              </v-dialog>
              <v-dialog
                v-model="dialogReporte"
                max-width="900px"
              >
                <v-card>
                  <v-card-text>
                    <iframe
                      :src="editedItem.id ? '/api/reports/ingresos/'+editedItem.id+'?format=pdf' : ''"
                      style="width: 100%;height: 500px;border: none;"
                    />
                  </v-card-text>
                  <v-card-actions>
                    <v-spacer />
                    <v-btn
                      color="blue darken-1"
                      text
                      @click="closeReporte"
                    >
                      Cerrar
                    </v-btn>
                    <v-spacer />
                  </v-card-actions>
                </v-card>
              </v-dialog>
            </v-toolbar>
          </template>
          <template v-slot:item.anulado="{ item }">
            <v-simple-checkbox
              v-model="item.anulado"
              disabled
            />
          </template>
          <template v-slot:item.cantidad="{ item }">
            <v-chip
              color="blue-grey"
              dark
            >
              {{ item.cantidad }}
            </v-chip>
          </template>
          <template v-slot:item.fecha="{ item }">
            <span>{{ new Date(item.fecha).toLocaleString() }}</span>
          </template>
          <template v-slot:expanded-item="{ headers, item }">
            <td :colspan="headers.length">
              <v-container>
                <v-row>
                  <v-col
                    cols="12"
                    md="6"
                  >
                    <v-alert
                      text
                      color="info"
                    >
                      <h3 class="text-h5">
                        Observaciones
                      </h3>
                      <div>{{ item.observaciones }}</div>
                    </v-alert>
                  </v-col>
                  <v-col
                    v-if="item.anulado"
                    cols="12"
                    md="6"
                  >
                    <v-alert
                      text
                      color="error"
                    >
                      <h3 class="text-h5">
                        Motivo de anulación
                      </h3>
                      <div>{{ item.motivoAnula }}</div>
                      <v-divider
                        class="my-4 info"
                        style="opacity: 0.22"
                      />
                      <v-row
                        align="center"
                        no-gutters
                      >
                        <v-col>
                          <h4 class="text-center">
                            Anulado por:
                          </h4>
                        </v-col>
                        <v-col>
                          <div>{{ item.usuarioAnula }}</div>
                        </v-col>
                        <v-col>
                          <h4 class="text-center">
                            Fecha:
                          </h4>
                        </v-col>
                        <v-col>
                          <div>{{ new Date(item.fechaAnula).toLocaleString() }}</div>
                        </v-col>
                      </v-row>
                    </v-alert>
                  </v-col>
                </v-row>
              </v-container>
            </td>
          </template>
          <template v-slot:item.actions="{ item }">
            <v-btn
              class="mx-2"
              fab
              dark
              x-small
              color="indigo"
              @click="reportItem(item)"
            >
              <v-icon>
                mdi-printer
              </v-icon>
            </v-btn>
            <v-btn
              v-if="!item.anulado"
              class="mx-2"
              fab
              dark
              x-small
              color="orange darken-4"
              @click="anularItem(item)"
            >
              <v-icon>
                mdi-cancel
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
  import { mapGetters } from 'vuex'

  export default {
    components: {
      FormIngreso,
    },
    data: () => ({
      dialog: false,
      dialogAnulacion: false,
      dialogReporte: false,
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
        { text: 'Fecha', value: 'fecha' },
        { text: 'Anulado', value: 'anulado' },
        { text: 'Acción', value: 'actions', sortable: false },
        { text: '', value: 'data-table-expand' },
      ],
      expanded: [],
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
        userAnulaId: null,
        usuario: '',
        usuarioAnula: '',
        observaciones: '',
        motivoAnula: null,
        anulado: false,
        cantidad: 0,
        fecha: '',
        fechaAnula: '',
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
        userAnulaId: null,
        usuario: '',
        usuarioAnulaId: '',
        observaciones: '',
        motivoAnula: null,
        anulado: false,
        cantidad: 0,
        fecha: '',
        fechaAnula: '',
      },
      snackbar: false,
      colorSnackbar: 'success',
      messageSnackbar: '',
      isAjaxPetitionInProgress: false,
    }),
    computed: {
      ...mapGetters({
        userGetter: 'auth/user',
      }),
    },
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
      reportItem (item) {
        this.editedIndex = this.ingresos.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialogReporte = true
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
          } else {
            this.$swal.fire(
              '¡Error!',
              response.response.data,
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
      anularItem (item) {
        this.editedIndex = this.ingresos.indexOf(item)
        this.editedItem = Object.assign({}, item)
        this.dialogAnulacion = true
      },
      async anulacionItemConfirm () {
        this.isAjaxPetitionInProgress = true
        this.editedItem.anulado = true
        this.editedItem.userAnulaId = this.userGetter.id
        this.editedItem.fechaAnula = new Date()
        const response = await IngresosService.edit(this.editedItem.id, this.editedItem)
        if (response.status >= 200 && response.status <= 299) {
          Object.assign(this.ingresos[this.editedIndex], this.editedItem)
          this.$swal.fire(
            '¡Exito!',
            'Se anulo el ingreso.',
            'success',
          )
        } else {
          this.$swal.fire(
            '¡Error!',
            response.response.data,
            'error',
          )
        }
        this.closeAnulacion()
        this.isAjaxPetitionInProgress = false
      },
      closeAnulacion () {
        this.dialogAnulacion = false
        this.$nextTick(() => {
          this.editedItem = Object.assign({}, this.defaultItem)
          this.editedIndex = -1
        })
      },
      closeReporte () {
        this.dialogReporte = false
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
          } else {
            this.$swal.fire(
              '¡Error!',
              response.response.data,
              'error',
            )
          }
        } else {
          const response = await IngresosService.create(this.editedItem)
          if (response.status === 201) {
            this.editedItem.id = response.data.id
            const nuevoIngresoResponse = await IngresosService.get(response.data.id)
            this.ingresos.push(nuevoIngresoResponse.data)
            this.$swal.fire(
              '¡Exito!',
              'Su registro ha sido creado.',
              'success',
            )
          } else {
            this.$swal.fire(
              '¡Error!',
              response.response.data,
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
