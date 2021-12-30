<template>
  <v-card>
    <v-card-title>Cronología</v-card-title>
    <v-divider />
    <v-card-text style="height: 60vh">
      <v-timeline
        dense
        clipped
      >
        <v-timeline-item
          fill-dot
          class="white--text mb-12"
          color="orange"
          large
        >
          <template v-slot:icon>
            <v-icon>
              mdi-timeline
            </v-icon>
          </template>
        </v-timeline-item>
        <v-slide-x-transition
          group
        >
          <v-timeline-item
            v-for="(event, i) in eventos"
            :key="i"
            class="mb-4"
            :color="getColor(event.descripcion)"
          >
            <template v-slot:icon>
              <v-icon>
                {{ getIcon(event.descripcion) }}
              </v-icon>
            </template>
            <v-row justify="space-between">
              <v-col
                cols="7"
                v-text="event.descripcion"
              />
              <v-col
                class="text-right"
                cols="5"
                v-text="new Date(event.fecha).toLocaleString()"
              />
            </v-row>
          </v-timeline-item>
        </v-slide-x-transition>
      </v-timeline>
    </v-card-text>
    <v-divider />
    <v-card-actions>
      <v-btn
        color="blue darken-1"
        text
        @click="close"
      >
        Cerrar
      </v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>

  export default {
    props: {
      eventos: {
        type: Array,
        default: function () {
          return []
        },
      },
    },
    data: () => ({

    }),
    methods: {
      close () {
        this.$emit('close')
      },
      getIcon (text) {
        if (text.includes('agrego')) {
          return 'mdi-clipboard-plus'
        } else if (text.includes('anulo')) {
          return 'mdi-clipboard-remove'
        } else if (text.includes('ingreso')) {
          return 'mdi-clipboard-arrow-down'
        } else if (text.includes('egreso')) {
          return 'mdi-clipboard-arrow-up'
        } else if (text.includes('edito')) {
          return 'mdi-clipboard-edit'
        }

        return 'mdi-clipboard'
      },
      getColor (text) {
        if (text.includes('agrego')) {
          return 'green'
        } else if (text.includes('anulo')) {
          return 'red'
        } else if (text.includes('ingreso')) {
          return 'blue'
        } else if (text.includes('egreso')) {
          return 'yellow'
        } else if (text.includes('edito')) {
          return 'purple'
        }

        return 'blue-grey'
      },
    },
  }
</script>
