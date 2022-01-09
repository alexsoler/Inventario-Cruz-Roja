<template>
  <div>
    Usuario: {{ userGetter.username }}
    Messages: {{ messages }}
  </div>
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
    },
  }
</script>

<style>

</style>
