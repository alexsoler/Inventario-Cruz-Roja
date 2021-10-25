import axios from '../plugins/http'

class InventariosService {
    async getAll (sedeId) {
      try {
        const response = await axios.get('/api/inventarios', { params: { sedeId } })

        return response
      } catch (error) {
        return error
      }
    }
}

export default new InventariosService()
