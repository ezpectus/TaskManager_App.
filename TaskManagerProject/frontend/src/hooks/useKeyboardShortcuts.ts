import { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'

export function useKeyboardShortcuts() {
  const navigate = useNavigate()

  useEffect(() => {
    const handler = (e: KeyboardEvent) => {
      if (e.target instanceof HTMLInputElement || e.target instanceof HTMLTextAreaElement || e.target instanceof HTMLSelectElement) return
      if (e.ctrlKey || e.metaKey || e.altKey) return

      switch (e.key.toLowerCase()) {
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
          break
        case 'd':
          navigate('/')
          break
        case 'k':
          navigate('/kanban')
          break
        case '?':
          alert(
            'Keyboard shortcuts:\n\n' +
            'N — New task\n' +
            '/ — Focus search\n' +
            'G then D — Go to Dashboard\n' +
            'G then K — Go to Kanban\n' +
            '? — Show this help'
          )
          break
      }
    }

    window.addEventListener('keydown', handler)
    return () => window.removeEventListener('keydown', handler)
  }, [navigate])
}
