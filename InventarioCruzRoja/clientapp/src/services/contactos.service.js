import axios from '../plugins/http'

class ContactosService {
    async getAll () {
      try {
        const response = await axios.get('/api/contactos')

        return response
      } catch (error) {
        return error
      }
    }

    async create (contacto) {
      try {
        const response = await axios.post('/api/contactos', contacto)

        return response
      } catch (error) {
        return error
      }
    }

    async edit (id, contacto) {
      try {
        const response = await axios.put(`/api/contactos/${id}`, contacto)

        return response
      } catch (error) {
        return error
      }
    }

    async delete (id) {
      try {
        const response = await axios.delete(`/api/contactos/${id}`)

        return response
      } catch (error) {
        return error
      }
    }
}

export default new ContactosService()
