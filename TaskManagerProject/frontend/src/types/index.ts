export type TaskStatus = 'Todo' | 'InProgress' | 'Done' | 'Cancelled'
export type TaskPriority = 'Low' | 'Medium' | 'High' | 'Critical'

export interface TaskDto {
  id: string
  title: string
  description: string
  status: TaskStatus
  priority: TaskPriority
  deadline: string
  createdAt: string
  updatedAt: string
  userId?: string | null
  username?: string | null
  subtasks: SubtaskDto[]
  comments: CommentDto[]
}

export interface SubtaskDto {
  id: string
  title: string
  isCompleted: boolean
  taskId: string
}

export interface CreateSubtaskRequest {
  title: string
  taskId: string
}

export interface UpdateSubtaskRequest {
  title?: string
  isCompleted?: boolean
}

export interface CommentDto {
  id: string
  content: string
  createdAt: string
  taskId: string
  userId?: string | null
  username?: string | null
}

export interface CreateCommentRequest {
  content: string
  taskId: string
  userId?: string | null
}

export interface UpdateCommentRequest {
  content: string
}

export interface CreateTaskRequest {
  title: string
  description: string
  priority: TaskPriority
  deadline?: string
  userId?: string | null
}

export interface UpdateTaskRequest {
  title?: string
  description?: string
  status?: TaskStatus
  priority?: TaskPriority
  deadline?: string
}

export interface RegisterRequest {
  username: string
  email: string
  password: string
}

export interface PagedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export interface UserDto {
  id: string
  username: string
  email: string
}

export interface LoginRequest {
  email: string
  password: string
}

export interface LoginResponse {
  token: string
  refreshToken: string
  expiresAt: string
  userId: string
  username: string
}

export interface RefreshTokenRequest {
  refreshToken: string
}

export interface ChangePasswordRequest {
  userId: string
  currentPassword: string
  newPassword: string
}

export interface UpdateUserRequest {
  username: string
  email: string
}
