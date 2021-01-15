import axios from '../plugins/http'

class FabricantesService {
    async getAll () {
      try {
        const response = await axios.get('/api/fabricantes')

        return response
      } catch (error) {
        return error
      }
    }

    async create (fabricante) {
      try {
        const response = await axios.post('/api/fabricantes', fabricante)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, fabricante) {
      try {
        const response = await axios.put(`/api/fabricantes/${id}`, fabricante)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/fabricantes/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new FabricantesService()
