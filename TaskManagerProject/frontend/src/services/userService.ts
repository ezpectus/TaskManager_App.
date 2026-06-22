import { apiFetch } from '../lib/api'
import type { UserDto } from '../types'

export const userService = {
  getById: (id: string) => apiFetch<UserDto>(`/users/${id}`),

  getAll: () => apiFetch<UserDto[]>('/users'),
}
