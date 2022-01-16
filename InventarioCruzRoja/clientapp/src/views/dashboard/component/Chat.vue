<template>
  <v-container class="grey lighten-5">
    <v-row>
      <v-col cols="12">
        <v-timeline
          align-top
          :dense="$vuetify.breakpoint.smAndDown"
        >
          <v-virtual-scroll
            :items="messages"
            :item-height="120"
            height="429"
          >
            <template v-slot:default="{ item }">
              <v-timeline-item
                :key="item.id"
                :color="getTimelineItemColor(item.userId)"
                :icon="getIconMessage(item.userId)"
                fill-dot
              >
                <v-card>
                  <v-card-title class="text-h5">
                    {{ item.userName }}
                  </v-card-title>
                  <v-card-text>
                    <p>{{ item.text }}</p>
                  </v-card-text>
                </v-card>
              </v-timeline-item>
            </template>
          </v-virtual-scroll>
        </v-timeline>
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <v-text-field
          v-model="message"
          append-outer-icon="mdi-send"
          prepend-icon="mdi-emoticon"
          filled
          clear-icon="mdi-close-circle"
          clearable
          label="Mensaje"
          type="text"
          @click:append-outer="sendMessage"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import MessagesService from '@/services/messages.service'
  import { mapGetters } from 'vuex'

  export default {
    data: () => ({
      messages: [],
      message: null,
    }),
    computed: {
      ...mapGetters({
        userGetter: 'auth/user',
      }),
    },
    created () {
      this.$chatHub.$on('receiveMessage', this.onReceiveMessage)
      console.log(this.$chatHub)
      this.getMessages()
    },
    beforeDestroy () {
      this.$chatHub.$off('receiveMessage', this.onReceiveMessage)
    },
    methods: {
      async getMessages () {
        const response = await MessagesService.getAll()
        if (response.status >= 200 && response.status <= 299) {
          this.messages = response.data
        }
      },
      async sendMessage () {
        if (this.message) {
          const parameters = {
            userName: this.userGetter.username,
            userId: this.userGetter.id,
            text: this.message,
          }

          const response = await MessagesService.create(parameters)
          if (response.status >= 200 && response.status <= 299) {
            this.messages.push(response.data)
          }
        }
      },
      onReceiveMessage (messageData) {
        if (messageData.userId !== this.userGetter.userId) {
          this.messages.push(messageData)
        }
      },
      getIconMessage (userId) {
        if (userId === this.userGetter.id) {
          return 'mdi-message-arrow-right'
        } else {
          return 'mdi-message-arrow-left'
        }
      },
      getTimelineItemColor (userId) {
        if (userId === this.userGetter.id) {
          return 'red lighten-2'
        } else {
          return 'indigo'
        }
      },
    },
  }
</script>

<style>

</style>
