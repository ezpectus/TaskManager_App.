import { apiFetch, setAuthToken, clearAuthToken } from '../lib/api'
import type { LoginRequest, LoginResponse, RefreshTokenRequest, RegisterRequest } from '../types'

export const authService = {
  register: async (data: RegisterRequest): Promise<void> => {
    await apiFetch('/users', {
      method: 'POST',
      body: JSON.stringify(data),
    })
  },

  login: async (data: LoginRequest): Promise<LoginResponse> => {
    const res = await apiFetch<LoginResponse>('/auth/login', {
      method: 'POST',
      body: JSON.stringify(data),
    })
    setAuthToken(res.token)
    localStorage.setItem('refreshToken', res.refreshToken)
    return res
  },

  refresh: async (): Promise<LoginResponse | null> => {
    const refreshToken = localStorage.getItem('refreshToken')
    if (!refreshToken) return null
    try {
      const res = await apiFetch<LoginResponse>('/auth/refresh', {
        method: 'POST',
        body: JSON.stringify({ refreshToken } as RefreshTokenRequest),
      })
      setAuthToken(res.token)
      localStorage.setItem('refreshToken', res.refreshToken)
      return res
    } catch {
      return null
    }
  },

  logout: async (): Promise<void> => {
    const refreshToken = localStorage.getItem('refreshToken')
    if (refreshToken) {
      try {
        await apiFetch('/auth/revoke', {
          method: 'POST',
          body: JSON.stringify({ refreshToken } as RefreshTokenRequest),
        })
      } catch {
        // ignore — token may already be invalid
      }
    }
    clearAuthToken()
    localStorage.removeItem('refreshToken')
  },
}
