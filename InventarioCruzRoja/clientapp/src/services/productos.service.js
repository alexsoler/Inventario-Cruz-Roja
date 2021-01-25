import axios from '../plugins/http'

class ProductosService {
    async getAll () {
      try {
        const response = await axios.get('/api/productos')

        return response
      } catch (error) {
        return error
      }
    }

    async get (id) {
      try {
        const response = await axios.get(`/api/productos/${id}`)

        return response
      } catch (error) {
        return error
      }
    }

    async create (producto) {
      try {
        const response = await axios.post('/api/productos', producto)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, producto) {
      try {
        const response = await axios.put(`/api/productos/${id}`, producto)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/productos/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new ProductosService()
