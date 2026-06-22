import { apiFetch } from '../lib/api'
import type { UserDto, UpdateUserRequest, ChangePasswordRequest } from '../types'

export const userService = {
  getById: (id: string) => apiFetch<UserDto>(`/users/${id}`),

  getAll: () => apiFetch<UserDto[]>('/users'),

  updateProfile: (id: string, data: UpdateUserRequest) =>
    apiFetch<void>(`/users/${id}/profile`, { method: 'PUT', body: JSON.stringify(data) }),

  changePassword: (id: string, data: ChangePasswordRequest) =>
    apiFetch<void>(`/users/${id}/change-password`, { method: 'POST', body: JSON.stringify(data) }),
}
