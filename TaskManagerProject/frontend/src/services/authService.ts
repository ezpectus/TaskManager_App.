import { apiFetch, setAuthToken, clearAuthToken } from '../lib/api'
import type { LoginRequest, LoginResponse } from '../types'

export const authService = {
  login: async (data: LoginRequest): Promise<void> => {
    const res = await apiFetch<LoginResponse>('/auth/login', {
      method: 'POST',
      body: JSON.stringify(data),
    })
    setAuthToken(res.token)
  },

  logout: () => {
    clearAuthToken()
  },
}
