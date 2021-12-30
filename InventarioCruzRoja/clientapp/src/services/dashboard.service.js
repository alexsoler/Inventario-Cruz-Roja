import axios from '../plugins/http'

class DashboardService {
  async getResumenProductos () {
    try {
      const response = await axios.get('/api/dashboard/ResumenDeProductos')

      return response
    } catch (error) {
      return error
    }
  }
}

export default new DashboardService()
