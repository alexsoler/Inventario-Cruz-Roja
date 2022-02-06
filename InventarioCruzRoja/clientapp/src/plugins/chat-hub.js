import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr'

export default {
  install (Vue) {
    const connection = new HubConnectionBuilder()
      .withUrl('/chatHub')
      .configureLogging(LogLevel.Information)
      .build()

    const chatHub = new Vue()

    Vue.prototype.$chatHub = chatHub

    connection.on('AddUserEvent', (userId, userName) => {
      chatHub.$emit('user-added-event', { userId, userName })
    })

    connection.on('receiveMessage', messageData => {
      chatHub.$emit('receiveMessage', messageData)
    })

    let startedPromise = null
    function start () {
      startedPromise = connection.start().catch(err => {
        console.error(err)
        return new Promise((resolve, reject) =>
          setTimeout(
            () =>
              start()
                .then(resolve)
                .catch(reject),
            5000,
          ),
        )
      })
      return startedPromise
    }

    connection.onclose(() => start())

    start()
  },
}
