import axios from 'axios'

// En Vercel define VITE_API_BASE_URL=https://tu-api.up.railway.app/api (Settings → Environment Variables)
const baseURL =
  "https://crmcontactcenter-production.up.railway.app";

const api = axios.create({
  baseURL,
  headers: {
    'Content-Type': 'application/json'
  }
})

// Interceptor — agrega el token JWT a cada petición automáticamente
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token')
  if (token) {
    config.headers.Authorization = `Bearer ${token}`
  }
  return config
})

// Interceptor — si el token expira redirige al login
api.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 401) {
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      window.location.href = '/login'
    }
    return Promise.reject(error)
  }
)

export default api
