import axios from '../plugins/http'

class MessagesService {
  async getAll () {
    try {
      const response = await axios.get('/api/messages')

      return response
    } catch (error) {
      return error
    }
  }

  async create (parameters) {
    try {
      const response = await axios.post('/api/messages', parameters)

      return response
    } catch (error) {
      return error
    }
  }
}

export default new MessagesService()
