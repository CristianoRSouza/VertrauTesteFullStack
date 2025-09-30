import axios from 'axios'

const api = axios.create({
  baseURL: 'http://localhost:5000/api'
})

api.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

export const authService = {
  login: (credentials) => api.post('/Auth/login', credentials),
  logout: () => localStorage.removeItem('token')
}

export const usuarioService = {
  create: (usuario) => api.post('/Usuarios', usuario),
  getAll: (page = 1, pageSize = 10, filtro = '') => api.get(`/Usuarios?page=${page}&pageSize=${pageSize}&filtro=${filtro}`),
  getById: (id) => api.get(`/Usuarios/${id}`),
  update: (id, usuario) => api.put(`/Usuarios/${id}`, usuario),
  delete: (id) => api.delete(`/Usuarios/${id}`)
}

export const cepService = {
  buscarCep: async (cep) => {
    const response = await axios.get(`https://viacep.com.br/ws/${cep}/json/`)
    return response.data
  }
}
