import { useState, useEffect } from 'react'
import { userService } from '../services/userService'
import { taskService } from '../services/taskService'
import type { UserDto, TaskDto } from '../types'
import { useToast } from '../context/ToastContext'
import { User as UserIcon, Mail, CheckSquare, Clock, CheckCircle } from 'lucide-react'

export default function ProfilePage() {
  const [user, setUser] = useState<UserDto | null>(null)
  const [tasks, setTasks] = useState<TaskDto[]>([])
  const [loading, setLoading] = useState(true)
  const { showToast } = useToast()

  useEffect(() => {
    const fetch = async () => {
      try {
        const token = localStorage.getItem('token')
        if (!token) return
        const payload = JSON.parse(atob(token.split('.')[1]))
        const userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.sub || payload.nameid
        if (!userId) return

        const [userData, taskData] = await Promise.all([
          userService.getById(userId),
          taskService.getAll(),
        ])
        setUser(userData)
        setTasks(taskData.filter((t) => t.userId === userId))
      } catch {
        showToast('Failed to load profile', 'error')
      } finally {
        setLoading(false)
      }
    }
    fetch()
  }, [])

  if (loading) {
    return (
      <div className="mx-auto max-w-2xl p-6">
        <div className="card p-6">
          <div className="skeleton mb-4 h-16 w-16 rounded-full" />
          <div className="skeleton mb-2 h-6 w-48" />
          <div className="skeleton mb-6 h-4 w-32" />
          <div className="skeleton h-20 w-full" />
        </div>
      </div>
    )
  }

  if (!user) {
    return (
      <div className="mx-auto max-w-2xl p-6">
        <div className="card p-6 text-center text-muted-foreground">
          User not found
        </div>
      </div>
    )
  }

  const todoCount = tasks.filter((t) => t.status === 'Todo').length
  const inProgressCount = tasks.filter((t) => t.status === 'InProgress').length
  const doneCount = tasks.filter((t) => t.status === 'Done').length

  return (
    <div className="mx-auto max-w-2xl p-6">
      <div className="card p-6">
        <div className="mb-6 flex items-center gap-4">
          <div className="flex h-16 w-16 items-center justify-center rounded-full bg-primary/10">
            <UserIcon className="h-8 w-8 text-primary" />
          </div>
          <div>
            <h1 className="text-2xl font-bold">{user.username}</h1>
            <p className="flex items-center gap-1 text-sm text-muted-foreground">
              <Mail size={14} /> {user.email}
            </p>
          </div>
        </div>

        <h2 className="mb-4 text-lg font-semibold">Task Statistics</h2>
        <div className="grid grid-cols-3 gap-4">
          <div className="card p-4 text-center">
            <CheckSquare className="mx-auto mb-2 h-6 w-6 text-blue-500" />
            <p className="text-2xl font-bold">{todoCount}</p>
            <p className="text-xs text-muted-foreground">To Do</p>
          </div>
          <div className="card p-4 text-center">
            <Clock className="mx-auto mb-2 h-6 w-6 text-yellow-500" />
            <p className="text-2xl font-bold">{inProgressCount}</p>
            <p className="text-xs text-muted-foreground">In Progress</p>
          </div>
          <div className="card p-4 text-center">
            <CheckCircle className="mx-auto mb-2 h-6 w-6 text-green-500" />
            <p className="text-2xl font-bold">{doneCount}</p>
            <p className="text-xs text-muted-foreground">Done</p>
          </div>
        </div>

        <h2 className="mb-4 mt-6 text-lg font-semibold">Recent Tasks</h2>
        {tasks.length === 0 ? (
          <p className="text-sm text-muted-foreground">No tasks assigned yet.</p>
        ) : (
          <div className="space-y-2">
            {tasks.slice(0, 5).map((task) => (
              <div key={task.id} className="card flex items-center justify-between p-3">
                <span className="font-medium">{task.title}</span>
                <span className={`badge ${
                  task.status === 'Done' ? 'bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200' :
                  task.status === 'InProgress' ? 'bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200' :
                  'bg-blue-100 text-blue-800 dark:bg-blue-900 dark:text-blue-200'
                }`}>
                  {task.status === 'InProgress' ? 'In Progress' : task.status}
                </span>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  )
}
