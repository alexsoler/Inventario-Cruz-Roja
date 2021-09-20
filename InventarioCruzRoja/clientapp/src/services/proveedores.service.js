import axios from '../plugins/http'

class ProveedoresService {
    async getAll () {
      try {
        const response = await axios.get('/api/proveedores')

        return response
      } catch (error) {
        return error
      }
    }

    async search (filter) {
      try {
        const response = await axios.get(`/api/proveedores?filter=${filter}`)

        return response
      } catch (error) {
        return error
      }
    }

    async create (proveedor) {
      try {
        const response = await axios.post('/api/proveedores', proveedor)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, proveedor) {
      try {
        const response = await axios.put(`/api/proveedores/${id}`, proveedor)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/proveedores/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new ProveedoresService()
