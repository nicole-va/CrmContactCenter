import { defineStore } from 'pinia'
import api from '../services/api'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('token') || null,
    user: JSON.parse(localStorage.getItem('user') || 'null')
  }),

  getters: {
    isAuthenticated: state => !!state.token,
    isAdmin: state => state.user?.role === 'Administrador',
    fullName: state => state.user?.fullName || ''
  },

  actions: {
    async login(email, password) {
      const response = await api.post('/api/auth/login', { email, password })
      const data = response.data

      this.token = data.token
      this.user = {
        fullName: data.fullName,
        email: data.email,
        role: data.role
      }

      localStorage.setItem('token', data.token)
      localStorage.setItem('user', JSON.stringify(this.user))
    },

    logout() {
      this.token = null
      this.user = null
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    }
  }
})
