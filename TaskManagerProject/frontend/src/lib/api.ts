export const API_BASE = '/api/v1'

export function getAuthToken(): string | null {
  return localStorage.getItem('token')
}

export function setAuthToken(token: string): void {
  localStorage.setItem('token', token)
}

export function clearAuthToken(): void {
  localStorage.removeItem('token')
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

  const res = await fetch(`${API_BASE}${path}`, {
    ...options,
    headers,
  })

  if (res.status === 401) {
    clearAuthToken()
    window.location.href = '/login'
    throw new Error('Unauthorized')
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
