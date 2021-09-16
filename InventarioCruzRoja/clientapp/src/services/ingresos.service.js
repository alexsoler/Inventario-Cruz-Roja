import axios from '../plugins/http'

class IngresosService {
    async getAll () {
      try {
        const response = await axios.get('/api/ingresos')

        return response
      } catch (error) {
        return error
      }
    }

    async create (ingreso) {
      try {
        const response = await axios.post('/api/ingresos', ingreso)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, ingreso) {
      try {
        const response = await axios.put(`/api/ingresos/${id}`, ingreso)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/ingresos/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new IngresosService()
