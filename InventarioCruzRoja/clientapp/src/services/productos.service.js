import axios from '../plugins/http'
import FuncionesService from './funciones.service'

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

    async create (producto, imagen) {
      try {
        const formData = FuncionesService.jsonToFormData(producto)

        formData.append('file', imagen)
        const response = await axios.post('/api/productos', formData, {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        })

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, producto, imagen) {
      try {
        const formData = FuncionesService.jsonToFormData(producto)

        if (imagen) {
          formData.append('file', imagen)
        }

        const response = await axios.put(`/api/productos/${id}`, formData, {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        })

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
