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
            color="pink"
            small
          >
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
    },
  }
</script>
