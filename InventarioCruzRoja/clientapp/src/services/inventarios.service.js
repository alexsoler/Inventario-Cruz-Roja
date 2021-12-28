import axios from '../plugins/http'

class InventariosService {
    async getAll (sedeId, fechaDesde, fechaHasta) {
      try {
        const response = await axios.get('/api/inventarios', { params: { sedeId, fechaDesde, fechaHasta } })

        return response
      } catch (error) {
        return error
      }
    }
}

export default new InventariosService()
