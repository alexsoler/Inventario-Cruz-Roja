import axios from '../plugins/http'

class CategoriasService {
    async getAll () {
      try {
        const response = await axios.get('/api/categorias')

        return response
      } catch (error) {
        return error
      }
    }

    async create (categoria) {
      try {
        const response = await axios.post('/api/categorias', categoria)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, categoria) {
      try {
        const response = await axios.put(`/api/categorias/${id}`, categoria)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/categorias/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new CategoriasService()
