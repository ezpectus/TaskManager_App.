import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { taskService } from '../services/taskService'
import type { TaskDto, TaskStatus } from '../types'
import { useToast } from '../context/ToastContext'
import { Plus, Trello } from 'lucide-react'

const COLUMNS: { status: TaskStatus; label: string; color: string }[] = [
  { status: 'Todo', label: 'To Do', color: 'border-t-blue-500' },
  { status: 'InProgress', label: 'In Progress', color: 'border-t-yellow-500' },
  { status: 'Done', label: 'Done', color: 'border-t-green-500' },
]

const PRIORITY_BADGE: Record<string, string> = {
  Low: 'bg-gray-100 text-gray-700 dark:bg-gray-800 dark:text-gray-300',
  Medium: 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-200',
  High: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200',
}

export default function KanbanPage() {
  const [tasks, setTasks] = useState<TaskDto[]>([])
  const [loading, setLoading] = useState(true)
  const navigate = useNavigate()
  const { showToast } = useToast()

  useEffect(() => {
    const fetch = async () => {
      try {
        const data = await taskService.getAll()
        setTasks(data)
      } catch {
        showToast('Failed to load tasks', 'error')
      } finally {
        setLoading(false)
      }
    }
    fetch()
  }, [])

  if (loading) {
    return (
      <div className="mx-auto max-w-7xl p-6">
        <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
          {[1, 2, 3].map((i) => (
            <div key={i} className="card min-h-[400px] border-t-4 border-t-muted p-4">
              <div className="skeleton mb-4 h-6 w-24" />
              {[1, 2].map((j) => (
                <div key={j} className="skeleton mb-3 h-20 w-full" />
              ))}
            </div>
          ))}
        </div>
      </div>
    )
  }

  return (
    <div className="mx-auto max-w-7xl p-6">
      <div className="mb-6 flex items-center justify-between">
        <h1 className="text-2xl font-bold">Kanban Board</h1>
        <button className="btn-primary" onClick={() => navigate('/')}>
          <Plus size={18} /> Dashboard
        </button>
      </div>

      <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
        {COLUMNS.map((col) => {
          const colTasks = tasks.filter((t) => t.status === col.status)
          return (
            <div key={col.status} className={`card border-t-4 ${col.color} min-h-[400px] p-4`}>
              <div className="mb-4 flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <Trello className="h-4 w-4 text-muted-foreground" />
                  <h2 className="font-semibold">{col.label}</h2>
                </div>
                <span className="badge bg-secondary text-secondary-foreground">{colTasks.length}</span>
              </div>
              <div className="space-y-3">
                {colTasks.map((task) => (
                  <div
                    key={task.id}
                    className="card cursor-pointer p-3 transition-shadow hover:shadow-md"
                    onClick={() => navigate(`/tasks/${task.id}`)}
                  >
                    <h3 className="mb-1 font-medium">{task.title}</h3>
                    <p className="mb-2 line-clamp-2 text-xs text-muted-foreground">{task.description}</p>
                    <div className="flex items-center justify-between">
                      <span className={`badge ${PRIORITY_BADGE[task.priority]}`}>{task.priority}</span>
                      {task.subtasks.length > 0 && (
                        <span className="text-xs text-muted-foreground">
                          {task.subtasks.filter((s) => s.isCompleted).length}/{task.subtasks.length} subtasks
                        </span>
                      )}
                    </div>
                  </div>
                ))}
                {colTasks.length === 0 && (
                  <p className="py-8 text-center text-sm text-muted-foreground">No tasks</p>
                )}
              </div>
            </div>
          )
        })}
      </div>
    </div>
  )
}
