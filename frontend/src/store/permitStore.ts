import { create } from 'zustand'

interface Permit {
  id: string
  number: string
  description: string
  startDate: string
  endDate: string
  status: string
  requestedBy: string
  approvedBy?: string
}

interface PermitStore {
  permits: Permit[]
  loading: boolean
  error: string | null
  fetchPermits: () => Promise<void>
  createPermit: (permit: Omit<Permit, 'id'>) => Promise<void>
  updatePermitStatus: (id: string, status: string) => Promise<void>
}

export const usePermitStore = create<PermitStore>((set) => ({
  permits: [],
  loading: false,
  error: null,
  fetchPermits: async () => {
    set({ loading: true })
    try {
      const token = localStorage.getItem('token')
      const response = await fetch(`${import.meta.env.VITE_API_URL}/permits`, {
        headers: { Authorization: `Bearer ${token}` },
      })
      const data = await response.json()
      set({ permits: data, loading: false })
    } catch (error) {
      set({ error: String(error), loading: false })
    }
  },
  createPermit: async (permit: Omit<Permit, 'id'>) => {
    try {
      const token = localStorage.getItem('token')
      await fetch(`${import.meta.env.VITE_API_URL}/permits`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(permit),
      })
    } catch (error) {
      set({ error: String(error) })
    }
  },
  updatePermitStatus: async (id: string, status: string) => {
    try {
      const token = localStorage.getItem('token')
      await fetch(`${import.meta.env.VITE_API_URL}/permits/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ status }),
      })
    } catch (error) {
      set({ error: String(error) })
    }
  },
}))
