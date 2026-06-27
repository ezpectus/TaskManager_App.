import { useState, useEffect, useCallback } from 'react'
import { useNavigate } from 'react-router-dom'
import { taskService } from '../services/taskService'
import type { TaskDto, TaskStatus } from '../types'
import { getRelativeDeadline, getSmartScore } from '../utils/deadline'
import { useToast } from '../context/ToastContext'
import { Plus, Trello, Calendar, ArrowUp, ArrowDown, Minus } from 'lucide-react'

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

const PRIORITY_ICONS: Record<string, React.ReactNode> = {
  High: <ArrowUp size={10} className="text-red-500" />,
  Medium: <Minus size={10} className="text-orange-500" />,
  Low: <ArrowDown size={10} className="text-gray-500" />,
}

export default function KanbanPage() {
  const [tasks, setTasks] = useState<TaskDto[]>([])
  const [loading, setLoading] = useState(true)
  const [draggedId, setDraggedId] = useState<string | null>(null)
  const [dragOverCol, setDragOverCol] = useState<TaskStatus | null>(null)
  const navigate = useNavigate()
  const { showToast } = useToast()

  const handleStatusChange = useCallback(async (taskId: string, newStatus: TaskStatus) => {
    try {
      await taskService.updateStatus(taskId, newStatus)
      setTasks((prev) => prev.map((t) => t.id === taskId ? { ...t, status: newStatus } : t))
      showToast('Task status updated', 'success')
    } catch {
      showToast('Failed to update status', 'error')
    }
  }, [showToast])

  const handleDragStart = (e: React.DragEvent, taskId: string) => {
    setDraggedId(taskId)
    e.dataTransfer.effectAllowed = 'move'
  }

  const handleDragOver = (e: React.DragEvent, col: TaskStatus) => {
    e.preventDefault()
    e.dataTransfer.dropEffect = 'move'
    setDragOverCol(col)
  }

  const handleDrop = (e: React.DragEvent, col: TaskStatus) => {
    e.preventDefault()
    setDragOverCol(null)
    if (draggedId) {
      handleStatusChange(draggedId, col)
      setDraggedId(null)
    }
  }

  const handleDragEnd = () => {
    setDraggedId(null)
    setDragOverCol(null)
  }

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
        <div className="flex items-center gap-3">
          <h1 className="text-2xl font-bold">Kanban Board</h1>
          {!loading && (
            <span className="badge bg-secondary text-secondary-foreground">{tasks.length}</span>
          )}
        </div>
        <button className="btn-primary" onClick={() => navigate('/')}>
          <Plus size={18} /> Dashboard
        </button>
      </div>

      <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
        {COLUMNS.map((col) => {
          const colTasks = tasks.filter((t) => t.status === col.status)
            .sort((a, b) => getSmartScore(b).total - getSmartScore(a).total)
          return (
            <div
              key={col.status}
              className={`card border-t-4 ${col.color} min-h-[400px] p-4 transition-colors ${dragOverCol === col.status ? 'ring-2 ring-primary' : ''}`}
              onDragOver={(e) => handleDragOver(e, col.status)}
              onDrop={(e) => handleDrop(e, col.status)}
              onDragLeave={() => setDragOverCol(null)}
            >
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
                    draggable
                    onDragStart={(e) => handleDragStart(e, task.id)}
                    onDragEnd={handleDragEnd}
                    className={`card cursor-pointer p-3 transition-all hover:shadow-md ${draggedId === task.id ? 'opacity-40' : ''}`}
                    onClick={() => navigate(`/tasks/${task.id}`)}
                  >
                    <h3 className="mb-1 font-medium">{task.title}</h3>
                    <p className="mb-2 line-clamp-2 text-xs text-muted-foreground">{task.description}</p>
                    {(() => {
                      const dl = getRelativeDeadline(task.deadline, task.status)
                      return dl ? (
                        <p className={`mb-2 flex items-center gap-1 text-xs ${dl.className}`}>
                          <Calendar size={11} />
                          {dl.text}
                        </p>
                      ) : null
                    })()}
                    <div className="flex items-center justify-between">
                      <span className={`badge ${PRIORITY_BADGE[task.priority]}`}>{PRIORITY_ICONS[task.priority]} {task.priority}</span>
                      <div className="flex items-center gap-1">
                        {task.subtasks.length > 0 && (
                          <span className="text-xs text-muted-foreground">
                            {task.subtasks.filter((s) => s.isCompleted).length}/{task.subtasks.length}
                          </span>
                        )}
                        <select
                          className="rounded border bg-transparent px-1 py-0.5 text-xs"
                          onClick={(e) => e.stopPropagation()}
                          onChange={(e) => handleStatusChange(task.id, e.target.value as TaskStatus)}
                          value={task.status}
                        >
                          <option value="Todo">Todo</option>
                          <option value="InProgress">In Progress</option>
                          <option value="Done">Done</option>
                        </select>
                      </div>
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
