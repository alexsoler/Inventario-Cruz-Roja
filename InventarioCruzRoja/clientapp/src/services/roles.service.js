import axios from '../plugins/http'

class RolesService {
    async getAll () {
      try {
        const { data } = await axios.get('/api/roles')

        return data
      } catch (error) {
        return error.response.data
      }
    }

    async create (role) {
      try {
        const { data } = await axios.post('/api/roles', role)

        return data
      } catch (error) {
        return error.response.data
      }
    }

    async edit (id, role) {
      try {
        const { data } = await axios.put(`/api/roles/${id}`, role)

        return data
      } catch (error) {
        return error.response.data
      }
    }

    async delete (id) {
      try {
        const { data } = await axios.delete(`/api/roles/${id}`)

        return data
      } catch (error) {
        return error.response.data
      }
    }
}

export default new RolesService()
