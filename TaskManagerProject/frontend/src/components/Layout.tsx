import { NavLink, useNavigate } from 'react-router-dom'
import { useAuth } from '../context/AuthContext'
import { useTheme } from '../context/ThemeContext'
import { LayoutDashboard, Trello, LogOut, CheckSquare, Moon, Sun, User, BarChart3, Keyboard } from 'lucide-react'
import KeyboardShortcutsOverlay from './KeyboardShortcutsOverlay'
import type { ReactNode } from 'react'

export default function Layout({ children }: { children: ReactNode }) {
  const { logout } = useAuth()
  const { theme, toggleTheme } = useTheme()
  const navigate = useNavigate()

  const handleLogout = () => {
    logout()
    navigate('/login')
  }

  const linkClass = ({ isActive }: { isActive: boolean }) =>
    `flex items-center gap-2 rounded-md px-3 py-2 text-sm font-medium transition-colors ${
      isActive ? 'bg-primary text-primary-foreground' : 'hover:bg-accent'
    }`

  return (
    <div className="min-h-screen bg-secondary">
      <header className="border-b bg-card">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-6 py-3">
          <div className="flex items-center gap-2">
            <CheckSquare className="h-6 w-6" />
            <span className="text-lg font-bold">TaskManager</span>
          </div>
          <nav className="flex items-center gap-2">
            <NavLink to="/" className={linkClass}>
              <LayoutDashboard size={18} /> Dashboard
            </NavLink>
            <NavLink to="/kanban" className={linkClass}>
              <Trello size={18} /> Kanban
            </NavLink>
            <NavLink to="/analytics" className={linkClass}>
              <BarChart3 size={18} /> Analytics
            </NavLink>
            <NavLink to="/profile" className={linkClass}>
              <User size={18} /> Profile
            </NavLink>
            <button onClick={toggleTheme} className="btn-outline ml-2" title="Toggle theme">
              {theme === 'dark' ? <Sun size={18} /> : <Moon size={18} />}
            </button>
            <button onClick={() => {}} className="btn-outline" title="Keyboard shortcuts (?)">
              <Keyboard size={18} />
            </button>
            <button onClick={handleLogout} className="btn-outline">
              <LogOut size={18} /> Logout
            </button>
          </nav>
        </div>
      </header>
      <main>{children}</main>
      <KeyboardShortcutsOverlay />
    </div>
  )
}
