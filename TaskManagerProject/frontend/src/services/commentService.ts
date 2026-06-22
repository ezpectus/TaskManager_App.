import { apiFetch } from '../lib/api'
import type { CommentDto, CreateCommentRequest } from '../types'

export const commentService = {
  getByTaskId: (taskId: string) => apiFetch<CommentDto[]>(`/comments/task/${taskId}`),

  create: (data: CreateCommentRequest) =>
    apiFetch<string>('/comments', {
      method: 'POST',
      body: JSON.stringify(data),
    }),

  update: (id: string, data: CreateCommentRequest) =>
    apiFetch<void>(`/comments/${id}`, {
      method: 'PUT',
      body: JSON.stringify(data),
    }),

  delete: (id: string) =>
    apiFetch<void>(`/comments/${id}`, {
      method: 'DELETE',
    }),
}
