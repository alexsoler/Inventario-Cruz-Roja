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

  async getResumenIngresos () {
    try {
      const response = await axios.get('/api/dashboard/ResumenDeIngresos')

      return response
    } catch (error) {
      return error
    }
  }

  async getResumenEgresos () {
    try {
      const response = await axios.get('/api/dashboard/ResumenDeEgresos')

      return response
    } catch (error) {
      return error
    }
  }

  async getCantidadDeUsuarios () {
    try {
      const response = await axios.get('/api/dashboard/CantidadDeUsuarios')

      return response
    } catch (error) {
      return error
    }
  }

  async getCantidadDeProductos () {
    try {
      const response = await axios.get('/api/dashboard/CantidadDeProductos')

      return response
    } catch (error) {
      return error
    }
  }

  async getCantidadDeSedes () {
    try {
      const response = await axios.get('/api/dashboard/CantidadDeSedes')

      return response
    } catch (error) {
      return error
    }
  }

  async getCantidadDeProveedores () {
    try {
      const response = await axios.get('/api/dashboard/CantidadDeProveedores')

      return response
    } catch (error) {
      return error
    }
  }

  async getUltimosEventos () {
    try {
      const response = await axios.get('/api/dashboard/UltimosEventos')

      return response
    } catch (error) {
      return error
    }
  }
}

export default new DashboardService()
