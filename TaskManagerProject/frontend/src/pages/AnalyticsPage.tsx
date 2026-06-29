import { useState, useEffect } from 'react'
import { taskService } from '../services/taskService'
import type { TaskDto, TaskStatus, TaskPriority } from '../types'
import { useToast } from '../context/ToastContext'
import { BarChart3, TrendingUp, CheckCircle, Clock, AlertCircle } from 'lucide-react'
import { isOverdue } from '../utils/deadline'

export default function AnalyticsPage() {
  const [tasks, setTasks] = useState<TaskDto[]>([])
  const [loading, setLoading] = useState(true)
  const { showToast } = useToast()

  useEffect(() => {
    const fetch = async () => {
      try {
        const data = await taskService.getAll()
        setTasks(data)
      } catch {
        showToast('Failed to load analytics', 'error')
      } finally {
        setLoading(false)
      }
    }
    fetch()
  }, [])

  if (loading) {
    return (
      <div className="mx-auto max-w-5xl p-6">
        <div className="card mb-6 h-8 w-48 skeleton" />
        <div className="grid grid-cols-1 gap-4 md:grid-cols-4">
          {[1, 2, 3, 4].map((i) => (
            <div key={i} className="card h-28 skeleton" />
          ))}
        </div>
        <div className="card mt-6 h-64 skeleton" />
      </div>
    )
  }

  const total = tasks.length
  const byStatus: Record<TaskStatus, number> = {
    Todo: tasks.filter((t) => t.status === 'Todo').length,
    InProgress: tasks.filter((t) => t.status === 'InProgress').length,
    Done: tasks.filter((t) => t.status === 'Done').length,
    Cancelled: tasks.filter((t) => t.status === 'Cancelled').length,
  }
  const byPriority: Record<TaskPriority, number> = {
    Low: tasks.filter((t) => t.priority === 'Low').length,
    Medium: tasks.filter((t) => t.priority === 'Medium').length,
    High: tasks.filter((t) => t.priority === 'High').length,
    Critical: tasks.filter((t) => t.priority === 'Critical').length,
  }
  const completionRate = total > 0 ? Math.round((byStatus.Done / total) * 100) : 0
  const overdue = tasks.filter((t) => isOverdue(t.deadline, t.status)).length

  const maxStatus = Math.max(...Object.values(byStatus), 1)
  const maxPriority = Math.max(...Object.values(byPriority), 1)

  return (
    <div className="mx-auto max-w-5xl p-6">
      <div className="mb-6 flex items-center gap-2">
        <BarChart3 className="h-6 w-6 text-primary" />
        <h1 className="text-2xl font-bold">Analytics</h1>
      </div>

      <div className="grid grid-cols-1 gap-4 md:grid-cols-4">
        <div className="card p-4">
          <div className="mb-2 flex items-center gap-2">
            <TrendingUp className="h-5 w-5 text-blue-500" />
            <span className="text-sm text-muted-foreground">Total Tasks</span>
          </div>
          <p className="text-3xl font-bold">{total}</p>
        </div>
        <div className="card p-4">
          <div className="mb-2 flex items-center gap-2">
            <CheckCircle className="h-5 w-5 text-green-500" />
            <span className="text-sm text-muted-foreground">Completion Rate</span>
          </div>
          <p className="text-3xl font-bold">{completionRate}%</p>
        </div>
        <div className="card p-4">
          <div className="mb-2 flex items-center gap-2">
            <Clock className="h-5 w-5 text-yellow-500" />
            <span className="text-sm text-muted-foreground">In Progress</span>
          </div>
          <p className="text-3xl font-bold">{byStatus.InProgress}</p>
        </div>
        <div className="card p-4">
          <div className="mb-2 flex items-center gap-2">
            <AlertCircle className="h-5 w-5 text-red-500" />
            <span className="text-sm text-muted-foreground">Overdue</span>
          </div>
          <p className="text-3xl font-bold">{overdue}</p>
        </div>
      </div>

      <div className="mt-6 grid grid-cols-1 gap-4 md:grid-cols-2">
        <div className="card p-6">
          <h2 className="mb-4 text-lg font-semibold">Tasks by Status</h2>
          <div className="space-y-3">
            {(['Todo', 'InProgress', 'Done', 'Cancelled'] as TaskStatus[]).map((status) => {
              const colors: Record<TaskStatus, string> = {
                Todo: 'bg-blue-500',
                InProgress: 'bg-yellow-500',
                Done: 'bg-green-500',
                Cancelled: 'bg-gray-500',
              }
              const labels: Record<TaskStatus, string> = {
                Todo: 'To Do',
                InProgress: 'In Progress',
                Done: 'Done',
                Cancelled: 'Cancelled',
              }
              return (
                <div key={status}>
                  <div className="mb-1 flex justify-between text-sm">
                    <span>{labels[status]}</span>
                    <span className="text-muted-foreground">{byStatus[status]}</span>
                  </div>
                  <div className="h-3 overflow-hidden rounded-full bg-secondary">
                    <div
                      className={`h-full rounded-full ${colors[status]} transition-all`}
                      style={{ width: `${(byStatus[status] / maxStatus) * 100}%` }}
                    />
                  </div>
                </div>
              )
            })}
          </div>
        </div>

        <div className="card p-6">
          <h2 className="mb-4 text-lg font-semibold">Tasks by Priority</h2>
          <div className="space-y-3">
            {(['Low', 'Medium', 'High', 'Critical'] as TaskPriority[]).map((priority) => {
              const colors: Record<TaskPriority, string> = {
                Low: 'bg-gray-500',
                Medium: 'bg-orange-500',
                High: 'bg-red-500',
                Critical: 'bg-purple-500',
              }
              return (
                <div key={priority}>
                  <div className="mb-1 flex justify-between text-sm">
                    <span>{priority}</span>
                    <span className="text-muted-foreground">{byPriority[priority]}</span>
                  </div>
                  <div className="h-3 overflow-hidden rounded-full bg-secondary">
                    <div
                      className={`h-full rounded-full ${colors[priority]} transition-all`}
                      style={{ width: `${(byPriority[priority] / maxPriority) * 100}%` }}
                    />
                  </div>
                </div>
              )
            })}
          </div>
        </div>
      </div>
    </div>
  )
}
