import axios from '../plugins/http'

class EstadosService {
    async getAll () {
      try {
        const response = await axios.get('/api/estados')

        return response
      } catch (error) {
        return error
      }
    }
}

export default new EstadosService()
