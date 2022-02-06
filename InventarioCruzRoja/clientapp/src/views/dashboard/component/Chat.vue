<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-timeline
          align-top
          dense
        >
          <v-virtual-scroll
            ref="scrollChat"
            :items="messages"
            :item-height="160"
            height="429"
          >
            <template v-slot:default="{ item }">
              <v-timeline-item
                :key="item.id"
                :color="getTimelineItemColor(item.userId)"
                :icon="getIconMessage(item.userId)"
                fill-dot
              >
                <v-card
                  :color="getMessageItemColor(item.userId)"
                  class="mr-2"
                >
                  <v-list-item>
                    <v-list-item-content>
                      <v-list-item-title class="text-h5">
                        {{ item.userName }}
                      </v-list-item-title>
                      <v-list-item-subtitle>{{ item.fecha | formatDate }}</v-list-item-subtitle>
                    </v-list-item-content>
                  </v-list-item>
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
          @click:prepend="toogleDialogEmoji"
        />
        <v-emoji-picker
          v-show="showDialog"
          :style="{ width: '440px', height: '200' }"
          label-search="Buscar"
          lang="es-HN"
          @select="onSelectEmoji"
        />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import { VEmojiPicker } from 'v-emoji-picker'
  import MessagesService from '@/services/messages.service'
  import { mapGetters } from 'vuex'

  export default {
    components: {
      VEmojiPicker,
    },
    data: () => ({
      messages: [],
      message: '',
      showDialog: false,
    }),
    computed: {
      ...mapGetters({
        userGetter: 'auth/user',
      }),
    },
    created () {
      this.$chatHub.$on('receiveMessage', this.onReceiveMessage)
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
          this.$nextTick(() => {
            this.goTheEndScroll()
          })
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
            this.message = ''
            this.messages.push(response.data)
            this.$nextTick(() => {
              this.goTheEndScroll()
            })
          }
        }
      },
      onReceiveMessage (messageData) {
        if (messageData.userId !== this.userGetter.id) {
          this.messages.push(messageData)
          this.$nextTick(() => {
            this.goTheEndScroll()
          })
        }
      },
      getIconMessage (userId) {
        if (userId === this.userGetter.id) {
          return 'mdi-message-arrow-left'
        } else {
          return 'mdi-message-arrow-right'
        }
      },
      getTimelineItemColor (userId) {
        if (userId === this.userGetter.id) {
          return 'indigo'
        } else {
          return 'red lighten-2'
        }
      },
      getMessageItemColor (userId) {
        if (userId === this.userGetter.id) {
          return this.$vuetify.theme.dark ? 'indigo darken-1' : 'indigo lighten-4'
        } else {
          return this.$vuetify.theme.dark ? 'red darken-1' : 'red lighten-5'
        }
      },
      goTheEndScroll () {
        this.$vuetify.goTo(9999, {
          duration: 300,
          offset: 0,
          easing: 'easeInOutCubic',
          container: this.$refs.scrollChat,
        })
      },
      toogleDialogEmoji () {
        this.showDialog = !this.showDialog
      },
      onSelectEmoji (emoji) {
        this.message += emoji.data
        this.toogleDialogEmoji()
      },
    },
  }
</script>

<style>

</style>
