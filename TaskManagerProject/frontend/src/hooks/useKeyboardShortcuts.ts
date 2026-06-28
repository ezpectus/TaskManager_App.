import { useEffect, useRef } from 'react'
import { useNavigate } from 'react-router-dom'

export function useKeyboardShortcuts() {
  const navigate = useNavigate()
  const pendingG = useRef(false)

  useEffect(() => {
    const handler = (e: KeyboardEvent) => {
      if (e.target instanceof HTMLInputElement || e.target instanceof HTMLTextAreaElement || e.target instanceof HTMLSelectElement) return
      if (e.ctrlKey || e.metaKey || e.altKey) return

      const key = e.key.toLowerCase()

      if (pendingG.current) {
        pendingG.current = false
        switch (key) {
          case 'd':
            navigate('/')
            return
          case 'k':
            navigate('/kanban')
            return
          case 'a':
            navigate('/analytics')
            return
          case 'p':
            navigate('/profile')
            return
        }
      }

      switch (key) {
        case 'n':
          navigate('/?new=true')
          break
        case '/':
          const searchInput = document.querySelector<HTMLInputElement>('input[placeholder*="Search"]')
          if (searchInput) {
            e.preventDefault()
            searchInput.focus()
          }
          break
        case 'g':
          pendingG.current = true
          setTimeout(() => { pendingG.current = false }, 1000)
          break
        case 'd':
          navigate('/')
          break
        case 'k':
          navigate('/kanban')
          break
        case '?':
          break
      }
    }

    window.addEventListener('keydown', handler)
    return () => window.removeEventListener('keydown', handler)
  }, [navigate])
}
