import { useState, useEffect, useCallback } from 'react'
import { taskService } from '../services/taskService'
import type { TaskDto, UpdateTaskRequest } from '../types'
import { useToast } from '../context/ToastContext'

export function useTask(id: string | undefined) {
  const [task, setTask] = useState<TaskDto | null>(null)
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const { showToast } = useToast()

  const fetchTask = useCallback(async () => {
    if (!id) return
    setLoading(true)
    setError(null)
    try {
      const data = await taskService.getById(id)
      setTask(data)
    } catch {
      setError('Failed to load task')
      showToast('Failed to load task', 'error')
    } finally {
      setLoading(false)
    }
  }, [id, showToast])

  const updateTask = useCallback(async (data: UpdateTaskRequest) => {
    if (!id) return false
    try {
      await taskService.update(id, data)
      showToast('Task updated', 'success')
      await fetchTask()
      return true
    } catch {
      showToast('Failed to update task', 'error')
      return false
    }
  }, [id, fetchTask, showToast])

  const deleteTask = useCallback(async () => {
    if (!id) return false
    try {
      await taskService.delete(id)
      showToast('Task deleted', 'success')
      return true
    } catch {
      showToast('Failed to delete task', 'error')
      return false
    }
  }, [id, showToast])

  useEffect(() => {
    fetchTask()
  }, [fetchTask])

  return { task, loading, error, fetchTask, updateTask, deleteTask }
}
