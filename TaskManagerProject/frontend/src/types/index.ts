export type TaskStatus = 'Todo' | 'InProgress' | 'Done'
export type TaskPriority = 'Low' | 'Medium' | 'High'

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

export interface CommentDto {
  id: string
  content: string
  createdAt: string
  taskId: string
  userId?: string | null
}

export interface CreateTaskRequest {
  title: string
  description: string
  priority: TaskPriority
  deadline: string
  userId?: string | null
}

export interface UpdateTaskRequest {
  title: string
  description: string
  priority: TaskPriority
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
}
