import { useState, useEffect, useMemo } from 'react'
import { useNavigate } from 'react-router-dom'
import { taskService } from '../services/taskService'
import type { TaskDto, TaskStatus, TaskPriority } from '../types'
import { useToast } from '../context/ToastContext'
import ConfirmDialog from '../components/ConfirmDialog'
import { useDebounce } from '../hooks/useDebounce'
import { exportTasksToCSV } from '../utils/csvExport'
import { getRelativeDeadline, isOverdue, isDueToday, getSmartScore, sortBySmartScore } from '../utils/deadline'
import { Plus, Search, Trash2, Edit, ClipboardList, LayoutGrid, Table, X, Download, Calendar, ArrowUp, ArrowDown, Minus, AlertCircle, ArrowUpDown } from 'lucide-react'

const STATUS_LABELS: Record<TaskStatus, string> = {
  Todo: 'To Do',
  InProgress: 'In Progress',
  Done: 'Done',
  Cancelled: 'Cancelled',
}

const STATUS_COLORS: Record<TaskStatus, string> = {
  Todo: 'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200',
  InProgress: 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200',
  Done: 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200',
  Cancelled: 'bg-gray-100 text-gray-800 dark:bg-gray-800 dark:text-gray-200',
}

const PRIORITY_COLORS: Record<string, string> = {
  Low: 'bg-gray-100 text-gray-700 dark:bg-gray-800 dark:text-gray-300',
  Medium: 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-200',
  High: 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200',
  Critical: 'bg-purple-100 text-purple-800 dark:bg-purple-900 dark:text-purple-200',
}

const PRIORITY_ICONS: Record<string, React.ReactNode> = {
  High: <ArrowUp size={12} className="text-red-500" />,
  Medium: <Minus size={12} className="text-orange-500" />,
  Low: <ArrowDown size={12} className="text-gray-500" />,
  Critical: <AlertCircle size={12} className="text-purple-500" />,
}

type SortBy = 'smart' | 'deadline' | 'priority' | 'created' | 'title'
type QuickFilter = 'overdue' | 'today' | 'highPriority' | null

const PRIORITY_WEIGHT: Record<string, number> = { Critical: 4, High: 3, Medium: 2, Low: 1 }

export default function DashboardPage() {
  const [tasks, setTasks] = useState<TaskDto[]>([])
  const [loading, setLoading] = useState(true)
  const [search, setSearch] = useState('')
  const [showCreate, setShowCreate] = useState(false)
  const [deleteTarget, setDeleteTarget] = useState<TaskDto | null>(null)
  const [view, setView] = useState<'grid' | 'table'>('grid')
  const [statusFilter, setStatusFilter] = useState<TaskStatus | null>(null)
  const [priorityFilter, setPriorityFilter] = useState<TaskPriority | null>(null)
  const [quickFilter, setQuickFilter] = useState<QuickFilter>(null)
  const [sortBy, setSortBy] = useState<SortBy>('smart')
  const navigate = useNavigate()
  const { showToast } = useToast()

  const fetchTasks = async () => {
    setLoading(true)
    try {
      const data = await taskService.getAll()
      setTasks(data)
    } catch {
      showToast('Failed to load tasks', 'error')
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    fetchTasks()
  }, [])

  const debouncedSearch = useDebounce(search, 300)

  const overdueCount = useMemo(() => tasks.filter((t) => isOverdue(t.deadline, t.status)).length, [tasks])
  const todayCount = useMemo(() => tasks.filter((t) => isDueToday(t.deadline, t.status)).length, [tasks])
  const highPriorityCount = useMemo(() => tasks.filter((t) => t.priority === 'High' && t.status !== 'Done').length, [tasks])

  const filtered = useMemo(() => {
    let result = tasks.filter(
      (t) =>
        (t.title.toLowerCase().includes(debouncedSearch.toLowerCase()) ||
        t.description.toLowerCase().includes(debouncedSearch.toLowerCase())) &&
        (statusFilter === null || t.status === statusFilter) &&
        (priorityFilter === null || t.priority === priorityFilter) &&
        (quickFilter === null ||
          (quickFilter === 'overdue' && isOverdue(t.deadline, t.status)) ||
          (quickFilter === 'today' && isDueToday(t.deadline, t.status)) ||
          (quickFilter === 'highPriority' && t.priority === 'High' && t.status !== 'Done')),
    )

    result = [...result].sort((a, b) => {
      switch (sortBy) {
        case 'smart':
          return getSmartScore(b).total - getSmartScore(a).total
        case 'deadline': {
          const aTime = new Date(a.deadline).getTime()
          const bTime = new Date(b.deadline).getTime()
          const aNone = a.deadline.startsWith('0001-01-01')
          const bNone = b.deadline.startsWith('0001-01-01')
          if (aNone && bNone) return 0
          if (aNone) return 1
          if (bNone) return -1
          return aTime - bTime
        }
        case 'priority':
          return PRIORITY_WEIGHT[b.priority] - PRIORITY_WEIGHT[a.priority]
        case 'title':
          return a.title.localeCompare(b.title)
        case 'created':
        default:
          return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
      }
    })

    return result
  }, [tasks, debouncedSearch, statusFilter, priorityFilter, quickFilter, sortBy])

  const activeFilters = [
    ...(statusFilter ? [{ type: 'status', label: STATUS_LABELS[statusFilter], clear: () => setStatusFilter(null) }] : []),
    ...(priorityFilter ? [{ type: 'priority', label: priorityFilter, clear: () => setPriorityFilter(null) }] : []),
    ...(quickFilter ? [{ type: 'quick', label: quickFilter === 'overdue' ? 'Overdue' : quickFilter === 'today' ? 'Due Today' : 'High Priority', clear: () => setQuickFilter(null) }] : []),
  ]

  const handleDelete = async (id: string) => {
    try {
      await taskService.delete(id)
      showToast('Task deleted', 'success')
      fetchTasks()
    } catch {
      showToast('Failed to delete task', 'error')
    }
  }

  const clearAllFilters = () => {
    setStatusFilter(null)
    setPriorityFilter(null)
    setQuickFilter(null)
  }

  return (
    <div className="mx-auto max-w-7xl p-6">
      <div className="mb-6 flex items-center justify-between">
        <div className="flex items-center gap-3">
          <h1 className="text-2xl font-bold">Tasks</h1>
          {!loading && (
            <span className="badge bg-secondary text-secondary-foreground">{filtered.length} / {tasks.length}</span>
          )}
        </div>
        <div className="flex gap-2">
          <button className="btn-outline" onClick={() => exportTasksToCSV(filtered)} disabled={filtered.length === 0}>
            <Download size={18} /> Export CSV
          </button>
          <button className="btn-primary" onClick={() => setShowCreate(true)}>
            <Plus size={18} /> New Task
          </button>
        </div>
      </div>

      <div className="mb-4 flex items-center gap-3">
        <div className="relative flex-1">
          <Search className="absolute left-3 top-3 h-4 w-4 text-muted-foreground" />
          <input
            className="input pl-10"
            placeholder="Search tasks..."
            value={search}
            onChange={(e) => setSearch(e.target.value)}
          />
        </div>
        <select
          className="input max-w-[180px]"
          value={sortBy}
          onChange={(e) => setSortBy(e.target.value as SortBy)}
          title="Sort by"
        >
          <option value="smart">Smart Priority (urgency × importance)</option>
          <option value="created">Newest first</option>
          <option value="deadline">By deadline</option>
          <option value="priority">By priority</option>
          <option value="title">By title (A-Z)</option>
        </select>
        <div className="flex items-center gap-1 rounded-lg border p-1">
          <button
            className={`rounded p-1.5 ${view === 'grid' ? 'bg-primary text-primary-foreground' : 'hover:bg-accent'}`}
            onClick={() => setView('grid')}
            title="Grid view"
          >
            <LayoutGrid size={16} />
          </button>
          <button
            className={`rounded p-1.5 ${view === 'table' ? 'bg-primary text-primary-foreground' : 'hover:bg-accent'}`}
            onClick={() => setView('table')}
            title="Table view"
          >
            <Table size={16} />
          </button>
        </div>
      </div>

      <div className="mb-4 flex flex-wrap items-center gap-2">
        <span className="text-sm text-muted-foreground">Quick filters:</span>
        <button
          className={`badge cursor-pointer ${quickFilter === 'overdue' ? 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200' : 'bg-secondary text-secondary-foreground hover:bg-accent'}`}
          onClick={() => setQuickFilter(quickFilter === 'overdue' ? null : 'overdue')}
        >
          <AlertCircle size={12} className="mr-1 inline" /> Overdue ({overdueCount})
        </button>
        <button
          className={`badge cursor-pointer ${quickFilter === 'today' ? 'bg-orange-100 text-orange-800 dark:bg-orange-900 dark:text-orange-200' : 'bg-secondary text-secondary-foreground hover:bg-accent'}`}
          onClick={() => setQuickFilter(quickFilter === 'today' ? null : 'today')}
        >
          <Calendar size={12} className="mr-1 inline" /> Due Today ({todayCount})
        </button>
        <button
          className={`badge cursor-pointer ${quickFilter === 'highPriority' ? 'bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200' : 'bg-secondary text-secondary-foreground hover:bg-accent'}`}
          onClick={() => setQuickFilter(quickFilter === 'highPriority' ? null : 'highPriority')}
        >
          <ArrowUp size={12} className="mr-1 inline" /> High Priority ({highPriorityCount})
        </button>
        <span className="mx-1 text-muted-foreground">|</span>
        {(['Todo', 'InProgress', 'Done', 'Cancelled'] as TaskStatus[]).map((s) => (
          <button
            key={s}
            className={`badge cursor-pointer ${statusFilter === s ? STATUS_COLORS[s] : 'bg-secondary text-secondary-foreground hover:bg-accent'}`}
            onClick={() => setStatusFilter(statusFilter === s ? null : s)}
          >
            {STATUS_LABELS[s]}
          </button>
        ))}
        <span className="mx-1 text-muted-foreground">|</span>
        {(['Low', 'Medium', 'High', 'Critical'] as TaskPriority[]).map((p) => (
          <button
            key={p}
            className={`badge cursor-pointer ${priorityFilter === p ? PRIORITY_COLORS[p] : 'bg-secondary text-secondary-foreground hover:bg-accent'}`}
            onClick={() => setPriorityFilter(priorityFilter === p ? null : p)}
          >
            {PRIORITY_ICONS[p]} {p}
          </button>
        ))}
        {activeFilters.length > 0 && (
          <button
            className="ml-2 flex items-center gap-1 text-xs text-muted-foreground hover:text-foreground"
            onClick={clearAllFilters}
          >
            <X size={12} /> Clear all
          </button>
        )}
      </div>

      {loading ? (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {[1, 2, 3, 4, 5, 6].map((i) => (
            <div key={i} className="card p-4">
              <div className="skeleton mb-3 h-5 w-3/4" />
              <div className="skeleton mb-2 h-4 w-full" />
              <div className="skeleton mb-4 h-4 w-2/3" />
              <div className="flex gap-2">
                <div className="skeleton h-5 w-16 rounded-full" />
                <div className="skeleton h-5 w-16 rounded-full" />
              </div>
            </div>
          ))}
        </div>
      ) : filtered.length === 0 ? (
        <div className="card p-12 text-center">
          <ClipboardList className="mx-auto mb-4 h-12 w-12 text-muted-foreground" />
          <p className="mb-2 text-lg font-medium">No tasks found</p>
          <p className="text-muted-foreground">
            {search || activeFilters.length > 0 ? 'Try adjusting your filters.' : 'Create a task to get started.'}
          </p>
        </div>
      ) : view === 'grid' ? (
        <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
          {filtered.map((task) => {
            const dl = getRelativeDeadline(task.deadline, task.status)
            return (
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
                        setDeleteTarget(task)
                      }}
                    >
                      <Trash2 size={16} className="text-destructive" />
                    </button>
                  </div>
                </div>
                <p className="mb-3 line-clamp-2 text-sm text-muted-foreground">
                  {task.description}
                </p>
                <div className="flex items-center justify-between">
                  <div className="flex gap-2">
                    <span className={`badge ${STATUS_COLORS[task.status]}`}>
                      {STATUS_LABELS[task.status]}
                    </span>
                    <span className={`badge ${PRIORITY_COLORS[task.priority]}`}>
                      {PRIORITY_ICONS[task.priority]} {task.priority}
                    </span>
                  </div>
                  {dl && (
                    <span className={`flex items-center gap-1 text-xs ${dl.className}`}>
                      <Calendar size={12} />
                      {dl.text}
                    </span>
                  )}
                </div>
              </div>
            )
          })}
        </div>
      ) : (
        <div className="card overflow-hidden">
          <table className="w-full text-sm">
            <thead className="border-b bg-secondary/50">
              <tr>
                <th className="px-4 py-3 text-left font-semibold">Title</th>
                <th className="px-4 py-3 text-left font-semibold">Status</th>
                <th className="px-4 py-3 text-left font-semibold">Priority</th>
                <th className="px-4 py-3 text-left font-semibold">Deadline</th>
                <th className="px-4 py-3 text-right font-semibold">Actions</th>
              </tr>
            </thead>
            <tbody>
              {filtered.map((task) => {
                const dl = getRelativeDeadline(task.deadline, task.status)
                return (
                  <tr
                    key={task.id}
                    className="cursor-pointer border-b last:border-0 hover:bg-accent/50"
                    onClick={() => navigate(`/tasks/${task.id}`)}
                  >
                    <td className="px-4 py-3">
                      <div className="font-medium">{task.title}</div>
                      <div className="line-clamp-1 text-xs text-muted-foreground">{task.description}</div>
                    </td>
                    <td className="px-4 py-3">
                      <span className={`badge ${STATUS_COLORS[task.status]}`}>{STATUS_LABELS[task.status]}</span>
                    </td>
                    <td className="px-4 py-3">
                      <span className={`badge ${PRIORITY_COLORS[task.priority]}`}>{PRIORITY_ICONS[task.priority]} {task.priority}</span>
                    </td>
                    <td className="px-4 py-3 text-xs">
                      {dl ? (
                        <span className={dl.className}>{dl.text}</span>
                      ) : (
                        <span className="text-muted-foreground">—</span>
                      )}
                    </td>
                    <td className="px-4 py-3">
                      <div className="flex justify-end gap-1">
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
                            setDeleteTarget(task)
                          }}
                        >
                          <Trash2 size={16} className="text-destructive" />
                        </button>
                      </div>
                    </td>
                  </tr>
                )
              })}
            </tbody>
          </table>
        </div>
      )}

      {showCreate && <CreateTaskModal onClose={() => { setShowCreate(false); fetchTasks() }} />}

      <ConfirmDialog
        open={!!deleteTarget}
        title="Delete task"
        message={`Are you sure you want to delete "${deleteTarget?.title}"? This action cannot be undone.`}
        confirmLabel="Delete"
        onConfirm={() => {
          if (deleteTarget) handleDelete(deleteTarget.id)
          setDeleteTarget(null)
        }}
        onCancel={() => setDeleteTarget(null)}
      />
    </div>
  )
}

function CreateTaskModal({ onClose }: { onClose: () => void }) {
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [priority, setPriority] = useState('Medium')
  const [deadline, setDeadline] = useState('')
  const [loading, setLoading] = useState(false)
  const { showToast } = useToast()

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    try {
      const token = localStorage.getItem('token')
      let userId: string | undefined
      if (token) {
        const payload = JSON.parse(atob(token.split('.')[1]))
        userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.sub || payload.nameid
      }
      await taskService.create({
        title,
        description,
        priority: priority as any,
        deadline: deadline || undefined,
        userId,
      })
      showToast('Task created successfully', 'success')
      onClose()
    } catch {
      showToast('Failed to create task', 'error')
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
                <option>Critical</option>
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
