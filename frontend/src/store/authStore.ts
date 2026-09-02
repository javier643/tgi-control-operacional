import { create } from 'zustand'

interface User {
  id: string
  username: string
  email: string
  role: string
  company: string
  operationalCenter: string
}

interface AuthStore {
  user: User | null
  token: string | null
  isAuthenticated: boolean
  login: (username: string, password: string, role: string) => Promise<void>
  logout: () => void
}

export const useAuthStore = create<AuthStore>((set) => ({
  user: null,
  token: null,
  isAuthenticated: false,
  login: async (username: string, password: string, role: string) => {
    try {
      const response = await fetch(`${import.meta.env.VITE_API_URL}/auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password, role }),
      })
      const data = await response.json()
      set({
        user: data.user,
        token: data.token,
        isAuthenticated: true,
      })
      localStorage.setItem('token', data.token)
    } catch (error) {
      console.error('Login failed:', error)
    }
  },
  logout: () => {
    set({ user: null, token: null, isAuthenticated: false })
    localStorage.removeItem('token')
  },
}))
