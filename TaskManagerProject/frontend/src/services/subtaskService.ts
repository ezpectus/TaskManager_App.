import { apiFetch } from '../lib/api'
import type { SubtaskDto, CreateSubtaskRequest, UpdateSubtaskRequest } from '../types'

export const subtaskService = {
  getByTaskId: (taskId: string) => apiFetch<SubtaskDto[]>(`/subtasks/task/${taskId}`),

  create: (data: CreateSubtaskRequest) =>
    apiFetch<string>('/subtasks', {
      method: 'POST',
      body: JSON.stringify(data),
    }),

  update: (id: string, data: UpdateSubtaskRequest) =>
    apiFetch<void>(`/subtasks/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    }),

  delete: (id: string) =>
    apiFetch<void>(`/subtasks/${id}`, {
      method: 'DELETE',
    }),
}
