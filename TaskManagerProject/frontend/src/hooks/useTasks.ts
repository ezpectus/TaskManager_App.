import { useState, useEffect, useCallback } from 'react'
import { taskService } from '../services/taskService'
import type { TaskDto, TaskStatus, TaskPriority } from '../types'
import { useToast } from '../context/ToastContext'

export function useTasks() {
  const [tasks, setTasks] = useState<TaskDto[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const { showToast } = useToast()

  const fetchTasks = useCallback(async () => {
    setLoading(true)
    setError(null)
    try {
      const data = await taskService.getAll()
      setTasks(data)
    } catch {
      setError('Failed to load tasks')
      showToast('Failed to load tasks', 'error')
    } finally {
      setLoading(false)
    }
  }, [showToast])

  const createTask = useCallback(async (title: string, description: string, priority: TaskPriority, deadline: string = '') => {
    try {
      const token = localStorage.getItem('token')
      let userId: string | undefined
      if (token) {
        const payload = JSON.parse(atob(token.split('.')[1]))
        userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] || payload.sub || payload.nameid
      }
      await taskService.create({ title, description, priority, deadline: deadline || undefined, userId })
      showToast('Task created', 'success')
      await fetchTasks()
    } catch {
      showToast('Failed to create task', 'error')
    }
  }, [fetchTasks, showToast])

  const deleteTask = useCallback(async (id: string) => {
    try {
      await taskService.delete(id)
      showToast('Task deleted', 'success')
      await fetchTasks()
    } catch {
      showToast('Failed to delete task', 'error')
    }
  }, [fetchTasks, showToast])

  const updateStatus = useCallback(async (id: string, status: TaskStatus) => {
    try {
      await taskService.updateStatus(id, status)
      setTasks((prev) => prev.map((t) => (t.id === id ? { ...t, status } : t)))
      showToast('Task status updated', 'success')
    } catch {
      showToast('Failed to update status', 'error')
    }
  }, [showToast])

  useEffect(() => {
    fetchTasks()
  }, [fetchTasks])

  return { tasks, loading, error, fetchTasks, createTask, deleteTask, updateStatus }
}
