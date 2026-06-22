export const API_BASE = '/api/v1'

export function getAuthToken(): string | null {
  return localStorage.getItem('token')
}

export function setAuthToken(token: string): void {
  localStorage.setItem('token', token)
}

export function clearAuthToken(): void {
  localStorage.removeItem('token')
  localStorage.removeItem('refreshToken')
}

let isRefreshing = false
let refreshPromise: Promise<boolean> | null = null

async function tryRefreshToken(): Promise<boolean> {
  const refreshToken = localStorage.getItem('refreshToken')
  if (!refreshToken) return false

  try {
    const res = await fetch(`${API_BASE}/auth/refresh`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ refreshToken }),
    })
    if (!res.ok) return false
    const data = await res.json()
    setAuthToken(data.token)
    localStorage.setItem('refreshToken', data.refreshToken)
    return true
  } catch {
    return false
  }
}

export async function apiFetch<T>(
  path: string,
  options: RequestInit = {},
): Promise<T> {
  const token = getAuthToken()
  const headers: Record<string, string> = {
    'Content-Type': 'application/json',
    ...((options.headers as Record<string, string>) || {}),
  }

  if (token) {
    headers['Authorization'] = `Bearer ${token}`
  }

  let res = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers,
  })

  if (res.status === 401 && !path.startsWith('/auth/')) {
    if (!isRefreshing) {
      isRefreshing = true
      refreshPromise = tryRefreshToken()
    }

    const refreshed = await refreshPromise!
    isRefreshing = false
    refreshPromise = null

    if (refreshed) {
      const newToken = getAuthToken()
      if (newToken) {
        headers['Authorization'] = `Bearer ${newToken}`
      }
      res = await fetch(`${API_BASE}${path}`, {
        ...options,
        headers,
      })
    }

    if (res.status === 401) {
      clearAuthToken()
      window.location.href = '/login'
      throw new Error('Unauthorized')
    }
  }

  if (!res.ok) {
    const error = await res.text()
    throw new Error(error || `HTTP ${res.status}`)
  }

  if (res.status === 204) {
    return undefined as T
  }

  return res.json() as Promise<T>
}
