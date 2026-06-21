import { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import { taskService } from '../services/taskService'
import type { TaskDto, TaskStatus } from '../types'
import { Plus, Search, Trash2, Edit } from 'lucide-react'

const STATUS_LABELS: Record<TaskStatus, string> = {
  Todo: 'To Do',
  InProgress: 'In Progress',
  Done: 'Done',
}

const STATUS_COLORS: Record<TaskStatus, string> = {
  Todo: 'bg-blue-100 text-blue-800',
  InProgress: 'bg-yellow-100 text-yellow-800',
  Done: 'bg-green-100 text-green-800',
}

const PRIORITY_COLORS: Record<string, string> = {
  Low: 'bg-gray-100 text-gray-700',
  Medium: 'bg-orange-100 text-orange-800',
  High: 'bg-red-100 text-red-800',
}

export default function DashboardPage() {
  const [tasks, setTasks] = useState<TaskDto[]>([])
  const [loading, setLoading] = useState(true)
  const [search, setSearch] = useState('')
  const [showCreate, setShowCreate] = useState(false)
  const navigate = useNavigate()

  const fetchTasks = async () => {
    setLoading(true)
    try {
      const data = await taskService.getAll()
      setTasks(data)
    } catch {
      // handle error
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchTasks()
  }, [])

  const filtered = tasks.filter(
    (t) =>
      t.title.toLowerCase().includes(search.toLowerCase()) ||
      t.description.toLowerCase().includes(search.toLowerCase()),
  )

  const handleDelete = async (id: string) => {
    if (!confirm('Delete this task?')) return
    await taskService.delete(id)
    fetchTasks()
  }

  return (
    <div className="mx-auto max-w-7xl p-6">
      <div className="mb-6 flex items-center justify-between">
        <h1 className="text-2xl font-bold">Tasks</h1>
        <button className="btn-primary" onClick={() => setShowCreate(true)}>
          <Plus size={18} /> New Task
        </button>
      </div>

      <div className="mb-4 relative">
        <Search className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
        <input
          className="input pl-10"
          placeholder="Search tasks..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      {loading ? (
        <p className="text-muted-foreground">Loading...</p>
      ) : filtered.length === 0 ? (
        <div className="card p-12 text-center">
          <p className="text-muted-foreground">No tasks found. Create one to get started.</p>
        </div>
      ) : (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {filtered.map((task) => (
            <div
              key={task.id}
              className="card cursor-pointer p-4 transition-shadow hover:shadow-md"
              onClick={() => navigate(`/tasks/${task.id}`)}
            >
              <div className="mb-2 flex items-start justify-between">
                <h3 className="font-semibold">{task.title}</h3>
                <div className="flex gap-1">
                  <button
                    className="rounded p-1 hover:bg-accent"
                    onClick={(e) => {
                      e.stopPropagation()
                      navigate(`/tasks/${task.id}?edit=true`)
                    }}
                  >
                    <Edit size={16} />
                  </button>
                  <button
                    className="rounded p-1 hover:bg-accent"
                    onClick={(e) => {
                      e.stopPropagation()
                      handleDelete(task.id)
                    }}
                  >
                    <Trash2 size={16} className="text-destructive" />
                  </button>
                </div>
              </div>
              <p className="mb-3 line-clamp-2 text-sm text-muted-foreground">
                {task.description}
              </p>
              <div className="flex gap-2">
                <span className={`badge ${STATUS_COLORS[task.status]}`}>
                  {STATUS_LABELS[task.status]}
                </span>
                <span className={`badge ${PRIORITY_COLORS[task.priority]}`}>
                  {task.priority}
                </span>
              </div>
            </div>
          ))}
        </div>
      )}

      {showCreate && <CreateTaskModal onClose={() => { setShowCreate(false); fetchTasks() }} />}
    </div>
  )
}

function CreateTaskModal({ onClose }: { onClose: () => void }) {
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [priority, setPriority] = useState('Medium')
  const [deadline, setDeadline] = useState('')
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    try {
      await taskService.create({
        title,
        description,
        priority: priority as any,
        deadline: deadline || new Date().toISOString(),
      })
      onClose()
    } catch {
      // handle error
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50" onClick={onClose}>
      <div className="card w-full max-w-md p-6" onClick={(e) => e.stopPropagation()}>
        <h2 className="mb-4 text-lg font-semibold">Create Task</h2>
        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="mb-1 block text-sm font-medium">Title</label>
            <input className="input" value={title} onChange={(e) => setTitle(e.target.value)} required autoFocus />
          </div>
          <div>
            <label className="mb-1 block text-sm font-medium">Description</label>
            <textarea className="input min-h-[80px]" value={description} onChange={(e) => setDescription(e.target.value)} required />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className="mb-1 block text-sm font-medium">Priority</label>
              <select className="input" value={priority} onChange={(e) => setPriority(e.target.value)}>
                <option>Low</option>
                <option>Medium</option>
                <option>High</option>
              </select>
            </div>
            <div>
              <label className="mb-1 block text-sm font-medium">Deadline</label>
              <input type="date" className="input" value={deadline} onChange={(e) => setDeadline(e.target.value)} />
            </div>
          </div>
          <div className="flex justify-end gap-2">
            <button type="button" className="btn-outline" onClick={onClose}>Cancel</button>
            <button type="submit" className="btn-primary" disabled={loading}>
              {loading ? 'Creating...' : 'Create'}
            </button>
          </div>
        </form>
      </div>
    </div>
  )
}
