import axios from 'axios'

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
    return await axios.post('/api/auth/register', {
      username: user.username,
      password: user.password,
    })
  }
}

export default new AuthService()
