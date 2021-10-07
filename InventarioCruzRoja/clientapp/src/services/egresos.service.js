import axios from '../plugins/http'

class EgresosService {
    async getAll () {
      try {
        const response = await axios.get('/api/egresos')

        return response
      } catch (error) {
        return error
      }
    }

    async get (id) {
      try {
        const response = await axios.get(`/api/egresos/${id}`)

        return response
      } catch (error) {
        return error
      }
    }

    async create (egreso) {
      try {
        const response = await axios.post('/api/egresos', egreso)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, egreso) {
      try {
        const response = await axios.put(`/api/egresos/${id}`, egreso)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/egresos/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new EgresosService()
