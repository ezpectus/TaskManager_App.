import { apiFetch } from '../lib/api'
import type {
  TaskDto,
  CreateTaskRequest,
  UpdateTaskRequest,
  PagedResult,
  TaskStatus,
  TaskPriority,
} from '../types'

export const taskService = {
  getAll: () => apiFetch<TaskDto[]>('/tasks'),

  getById: (id: string) => apiFetch<TaskDto>(`/tasks/${id}`),

  getFiltered: (params: {
    page?: number
    pageSize?: number
    status?: TaskStatus
    priority?: TaskPriority
    userId?: string
    searchTerm?: string
  }) => {
    const query = new URLSearchParams()
    if (params.page) query.set('page', String(params.page))
    if (params.pageSize) query.set('pageSize', String(params.pageSize))
    if (params.status) query.set('status', params.status)
    if (params.priority) query.set('priority', params.priority)
    if (params.userId) query.set('userId', params.userId)
    if (params.searchTerm) query.set('searchTerm', params.searchTerm)
    const qs = query.toString()
    return apiFetch<PagedResult<TaskDto>>(`/tasks${qs ? `?${qs}` : ''}`)
  },

  create: (data: CreateTaskRequest) =>
    apiFetch<string>('/tasks', {
      method: 'POST',
      body: JSON.stringify(data),
    }),

  update: (id: string, data: UpdateTaskRequest) =>
    apiFetch<void>(`/tasks/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    }),

  updateStatus: (id: string, status: TaskStatus) =>
    apiFetch<void>(`/tasks/${id}`, {
      method: 'PUT',
      body: JSON.stringify({ status }),
    }),

  delete: (id: string) =>
    apiFetch<void>(`/tasks/${id}`, {
      method: 'DELETE',
    }),
}
