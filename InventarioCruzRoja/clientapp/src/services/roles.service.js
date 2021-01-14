import axios from '../plugins/http'

class RolesService {
    async getAll () {
      try {
        const response = await axios.get('/api/roles')

        return response
      } catch (error) {
        return error
      }
    }

    async create (role) {
      try {
        const response = await axios.post('/api/roles', role)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, role) {
      try {
        const response = await axios.put(`/api/roles/${id}`, role)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/roles/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new RolesService()
