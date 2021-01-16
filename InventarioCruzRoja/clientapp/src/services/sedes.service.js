import axios from '../plugins/http'

class SedesService {
    async getAll () {
      try {
        const response = await axios.get('/api/sedes')

        return response
      } catch (error) {
        return error
      }
    }

    async create (sede) {
      try {
        const response = await axios.post('/api/sedes', sede)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, sede) {
      try {
        const response = await axios.put(`/api/sedes/${id}`, sede)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/sedes/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new SedesService()
