import { useEffect, useState } from 'react'
import { X, Keyboard } from 'lucide-react'

const SHORTCUTS = [
  { keys: ['G', 'D'], description: 'Go to Dashboard' },
  { keys: ['G', 'K'], description: 'Go to Kanban board' },
  { keys: ['G', 'A'], description: 'Go to Analytics' },
  { keys: ['G', 'P'], description: 'Go to Profile' },
  { keys: ['N'], description: 'New task (on Dashboard)' },
  { keys: ['/'], description: 'Focus search (on Dashboard)' },
  { keys: ['Esc'], description: 'Close dialogs / cancel edit' },
  { keys: ['?'], description: 'Toggle this shortcuts panel' },
]

export default function KeyboardShortcutsOverlay() {
  const [open, setOpen] = useState(false)

  useEffect(() => {
    const handler = (e: KeyboardEvent) => {
      if (e.key === '?' && !['INPUT', 'TEXTAREA', 'SELECT'].includes((e.target as HTMLElement)?.tagName)) {
        e.preventDefault()
        setOpen((prev) => !prev)
      }
      if (e.key === 'Escape') {
        setOpen(false)
      }
    }
    window.addEventListener('keydown', handler)
    return () => window.removeEventListener('keydown', handler)
  }, [])

  if (!open) return null

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center bg-black/50"
      onClick={() => setOpen(false)}
    >
      <div
        className="card w-full max-w-md p-6"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="mb-4 flex items-center justify-between">
          <div className="flex items-center gap-2">
            <Keyboard className="h-5 w-5" />
            <h2 className="text-lg font-semibold">Keyboard Shortcuts</h2>
          </div>
          <button className="rounded p-1 hover:bg-accent" onClick={() => setOpen(false)}>
            <X size={18} />
          </button>
        </div>
        <div className="space-y-2">
          {SHORTCUTS.map((s, i) => (
            <div key={i} className="flex items-center justify-between text-sm">
              <span className="text-muted-foreground">{s.description}</span>
              <div className="flex gap-1">
                {s.keys.map((k, j) => (
                  <span key={j}>
                    {j > 0 && <span className="mx-1 text-muted-foreground">then</span>}
                    <kbd className="rounded border bg-secondary px-2 py-0.5 text-xs font-mono">
                      {k}
                    </kbd>
                  </span>
                ))}
              </div>
            </div>
          ))}
        </div>
        <p className="mt-4 text-xs text-muted-foreground">
          Press <kbd className="rounded border bg-secondary px-1.5 py-0.5 font-mono">?</kbd> anywhere to toggle this panel.
        </p>
      </div>
    </div>
  )
}
