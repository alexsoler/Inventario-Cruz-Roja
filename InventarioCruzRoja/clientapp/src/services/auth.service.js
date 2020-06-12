import axios from 'axios'
import authHeader from './auth-header'

class AuthService {
  async login (user) {
    const { data: { data: response } } = await axios.post('/api/auth/login', {
      username: user.username,
      password: user.password,
    })

    if (response) {
      localStorage.setItem('user', JSON.stringify(response))
    }
    return response
  }

  logout () {
    localStorage.removeItem('user')
  }

  async register (user) {
    try {
      const { data } = await axios.post('/api/auth/register', user, {
        headers: authHeader(),
      })

      return data
    } catch (error) {
      return error.response.data
    }
  }

  async edit (id, user) {
    try {
      await axios.put(`/api/auth/edit/${id}`, user, {
        headers: authHeader(),
      })

      return true
    } catch (error) {
      return false
    }
  }

  async delete (id) {
    try {
      const { data } = await axios.delete(`/api/auth/delete/${id}`, {
        headers: authHeader(),
      })

      return data
    } catch (error) {
      return error.response.data
    }
  }

  async getAllRoles () {
    const { data } = await axios.get('/api/auth/roles', {
      headers: authHeader(),
    })

    return data
  }

  async getAllUsers () {
    const { data } = await axios.get('/api/auth/users', {
      headers: authHeader(),
    })

    return data
  }
}

export default new AuthService()
