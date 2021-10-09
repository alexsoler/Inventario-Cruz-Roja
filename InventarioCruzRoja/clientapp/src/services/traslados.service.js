import axios from '../plugins/http'

class TrasladosService {
    async getAll () {
      try {
        const response = await axios.get('/api/traslados')

        return response
      } catch (error) {
        return error
      }
    }

    async get (id) {
      try {
        const response = await axios.get(`/api/traslados/${id}`)

        return response
      } catch (error) {
        return error
      }
    }

    async create (traslado) {
      try {
        const response = await axios.post('/api/traslados', traslado)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, traslado) {
      try {
        const response = await axios.put(`/api/traslados/${id}`, traslado)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/traslados/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new TrasladosService()
