<template>
  <v-card>
    <div class="text-center">
      <vue-barcode
        v-model="codigo"
        :format="config.format"
      />
    </div>

    <v-card-text>
      <v-container>
        <v-row>
          <v-col
            cols="12"
            sm="6"
          >
            <v-text-field
              v-model.number="countTags"
              label="Cantidad de Etiquetas"
              required
              outlined
              type="number"
            />
          </v-col>
          <v-col
            cols="12"
            sm="6"
          >
            <v-select
              v-model="config.format"
              :items="formatos"
              label="Formatos"
              outlined
            />
          </v-col>
        </v-row>
      </v-container>
    </v-card-text>

    <v-card-actions>
      <v-spacer />
      <v-btn
        color="orange"
        text
        @click="close"
      >
        Cerrar
      </v-btn>

      <v-btn
        color="orange"
        text
        @click="generarCodebars"
      >
        Generar
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
  import VueBarcode from 'vue-barcode'
  import { generateAndDownloadBarcodeInPDF } from '@/services/generate-barcode.service'

  export default {
    components: {
      VueBarcode,
    },
    props: {
      codigo: {
        type: String,
        default: '',
      },
    },
    data: () => ({
      config: {
        format: 'CODE128',
      },
      countTags: 1,
      formatos: ['CODE128', 'CODE128A', 'CODE128B', 'CODE128C', 'EAN13', 'EAN8', 'EAN5', 'EAN2',
                 'UPC', 'CODE39', 'ITF14', 'ITF', 'MSI', 'MSI10', 'MSI11', 'MSI1010', 'MSI1110', 'Pharmacode', 'Codabar'],
    }),
    methods: {
      generarCodebars () {
        generateAndDownloadBarcodeInPDF(this.codigo, this.countTags, this.config)
      },
      close () {
        this.$emit('close')
      },
    },
  }
</script>

<style>

</style>
