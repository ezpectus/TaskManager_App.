import { createContext, useContext, useState, type ReactNode } from 'react'
import { getAuthToken } from '../lib/api'
import { authService } from '../services/authService'

interface AuthContextValue {
  isAuthenticated: boolean
  login: (token: string) => void
  logout: () => Promise<void>
}

const AuthContext = createContext<AuthContextValue | null>(null)

export function AuthProvider({ children }: { children: ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState(!!getAuthToken())

  const login = (_token: string) => {
    setIsAuthenticated(true)
  }

  const logout = async () => {
    await authService.logout()
    setIsAuthenticated(false)
  }

  return (
    <AuthContext.Provider value={{ isAuthenticated, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}

export function useAuth() {
  const ctx = useContext(AuthContext)
  if (!ctx) throw new Error('useAuth must be used within AuthProvider')
  return ctx
}
