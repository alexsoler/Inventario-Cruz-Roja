import axios from 'axios'
import store from '../store'
import { router } from '../router'
import authHeader from '../services/auth-header'

const http = axios.create()

http.interceptors.request.use(config => {
    config.headers = authHeader()

    return config
}, error => {
    Promise.reject(error)
})

http.interceptors.response.use(response => {
    // Do something with response data
    return response
}, (error) => {
    // Do something with response error
    if (error.response.status === 401) {
        store.dispatch('auth/logout')
        router.push('login')
    }
    return Promise.reject(error)
})

export default http
