import { apiFetch, setAuthToken, clearAuthToken } from '../lib/api'
import type { LoginRequest, LoginResponse } from '../types'

export const authService = {
  login: async (data: LoginRequest): Promise<string> => {
    const res = await apiFetch<LoginResponse>('/auth/login', {
      method: 'POST',
      body: JSON.stringify(data),
    })
    setAuthToken(res.token)
    return res.token
  },

  logout: () => {
    clearAuthToken()
  },
}
