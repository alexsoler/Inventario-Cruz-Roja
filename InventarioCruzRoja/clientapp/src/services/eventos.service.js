import axios from '../plugins/http'

class EventosService {
  async getProductEvents (id) {
    try {
      const response = await axios.get(`/api/eventos/producto/${id}`)

      return response
    } catch (error) {
      return error
    }
  }
}

export default new EventosService()
