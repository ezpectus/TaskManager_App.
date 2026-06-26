# API Documentation

Base URL: `http://localhost:5000/api/v1/`

All responses use JSON. All write endpoints require `Authorization: Bearer <JWT>` header unless marked as **Public**.

## Error Responses

All errors follow a consistent format:

```json
{
  "error": {
    "code": "ValidationError",
    "message": "One or more validation errors occurred.",
    "details": [
      "Title must not be empty.",
      "Description must not exceed 2000 characters."
    ]
  }
}
```

| Status Code | Meaning |
|-------------|---------|
| 200 | Success |
| 201 | Created (with `Location` header) |
| 204 | No Content (success, empty response) |
| 400 | Bad Request (validation error) |
| 401 | Unauthorized (missing or invalid token) |
| 404 | Not Found |
| 409 | Conflict (concurrency violation) |
| 500 | Internal Server Error |

---

## Authentication

### POST /auth/login

Public endpoint. Authenticates a user and returns JWT + refresh token.

**Request:**
```json
{
  "email": "admin@taskmanager.com",
  "password": "Admin123!"
}
```

**Response (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "a1b2c3d4-e5f6-...",
  "user": {
    "id": "guid",
    "username": "admin",
    "email": "admin@taskmanager.com"
  }
}
```

**Response (401):** Invalid credentials

---

### POST /auth/refresh

Public endpoint. Exchanges a refresh token for a new JWT + refresh token pair.

**Request:**
```json
{
  "refreshToken": "a1b2c3d4-e5f6-..."
}
```

**Response (200):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "new-refresh-token-guid",
  "user": {
    "id": "guid",
    "username": "admin",
    "email": "admin@taskmanager.com"
  }
}
```

**Response (401):** Invalid or expired refresh token

---

## Tasks

### POST /tasks

Creates a new task.

**Request:**
```json
{
  "title": "Implement login page",
  "description": "Add JWT-based login form with validation",
  "priority": "High",
  "deadline": "2026-07-01T00:00:00Z"
}
```

**Response (201):** `Location: /api/v1/tasks/{id}`

---

### GET /tasks

Retrieves tasks with optional pagination and filtering.

**Query Parameters:**

| Parameter | Type | Default | Description |
|-----------|------|---------|-------------|
| `page` | int | 1 | Page number |
| `pageSize` | int | 20 | Items per page (max 100) |
| `status` | string | null | Filter by status: `Todo`, `InProgress`, `Done`, `Cancelled` |
| `priority` | string | null | Filter by priority: `Low`, `Medium`, `High`, `Critical` |
| `userId` | guid | null | Filter by assigned user |
| `searchTerm` | string | null | Full-text search on title and description |

**Response (200) — with pagination:**
```json
{
  "items": [
    {
      "id": "guid",
      "title": "Implement login page",
      "description": "Add JWT-based login form",
      "status": "Todo",
      "priority": "High",
      "createdAt": "2026-06-01T12:00:00Z",
      "updatedAt": "2026-06-01T12:00:00Z",
      "subtasks": [],
      "comments": []
    }
  ],
  "totalCount": 42,
  "page": 1,
  "pageSize": 20,
  "totalPages": 3
}
```

**Response (200) — without pagination (all tasks):** Array of `TaskDto`

---

### GET /tasks/{id}

Retrieves a single task by ID. Response cached for 30 seconds.

**Response (200):** `TaskDto` with subtasks, comments, and attachments

**Response (404):** Task not found

---

### PUT /tasks/{id}

Updates a task. Only provided fields are updated (partial update).

**Request:**
```json
{
  "title": "Updated title",
  "status": "InProgress",
  "priority": "Critical"
}
```

**Response (204):** Success, no content  
**Response (404):** Task not found  
**Response (409):** Concurrency conflict (RowVersion mismatch)

---

### DELETE /tasks/{id}

Soft deletes a task (sets `IsDeleted = true`, `DeletedAt = now`).

**Response (204):** Success  
**Response (404):** Task not found

---

### POST /tasks/{id}/assign/{userId}

Assigns a task to a user.

**Response (204):** Success  
**Response (404):** Task or user not found

---

### GET /tasks/{id}/activity

Retrieves activity log entries for a specific task.

**Response (200):**
```json
[
  {
    "id": "guid",
    "actionType": "TaskCreated",
    "timestamp": "2026-06-01T12:00:00Z",
    "taskId": "guid",
    "userId": "guid"
  }
]
```

---

## Subtasks

### POST /subtasks

**Request:**
```json
{
  "title": "Write unit tests",
  "taskId": "guid"
}
```

**Response (201):** `Location: /api/v1/subtasks/{id}`

---

### GET /subtasks/{id}

**Response (200):** `SubtaskDto`  
**Response (404):** Not found

---

### GET /subtasks/task/{taskId}

Retrieves all subtasks for a given task.

**Response (200):** Array of `SubtaskDto`

---

### PUT /subtasks/{id}

**Request:**
```json
{
  "title": "Updated subtask title",
  "isCompleted": true
}
```

**Response (204):** Success  
**Response (404):** Not found

---

### DELETE /subtasks/{id}

Soft deletes a subtask.

**Response (204):** Success  
**Response (404):** Not found

---

## Comments

### POST /comments

**Request:**
```json
{
  "content": "This looks good, but needs more tests.",
  "taskId": "guid",
  "userId": "guid"
}
```

**Response (201):** `Location: /api/v1/comments/{id}`

---

### GET /comments/{id}

**Response (200):** `CommentDto`  
**Response (404):** Not found

---

### GET /comments/task/{taskId}

**Response (200):** Array of `CommentDto`

---

### PUT /comments/{id}

**Request:**
```json
{
  "content": "Updated comment text",
  "taskId": "guid",
  "userId": "guid"
}
```

**Response (204):** Success  
**Response (404):** Not found

---

### DELETE /comments/{id}

Soft deletes a comment.

**Response (204):** Success  
**Response (404):** Not found

---

## Users

### GET /users

Retrieves all users. Response cached for 60 seconds.

**Response (200):** Array of `UserDto`

---

### GET /users/{id}

**Response (200):** `UserDto`  
**Response (404):** Not found

---

### POST /users

Public endpoint (registration).

**Request:**
```json
{
  "username": "newuser",
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

**Response (201):** `Location: /api/v1/users/{id}`

---

### PUT /users/{id}

**Request:**
```json
{
  "username": "updatedname",
  "email": "newemail@example.com"
}
```

**Response (204):** Success  
**Response (404):** Not found

---

### PUT /users/{id}/profile

Updates user profile (username, email) — separate from full user update.

**Request:**
```json
{
  "username": "newusername",
  "email": "newemail@example.com"
}
```

**Response (204):** Success  
**Response (404):** Not found

---

### POST /users/{id}/change-password

Changes user password. Requires current password verification.

**Request:**
```json
{
  "userId": "guid",
  "currentPassword": "OldPass123!",
  "newPassword": "NewPass456!"
}
```

**Response (204):** Success  
**Response (400):** Current password incorrect or user not found

---

### DELETE /users/{id}

**Response (204):** Success  
**Response (404):** Not found

---

## Roles

### GET /roles

**Response (200):** Array of `RoleDto`

### GET /roles/{id}

**Response (200):** `RoleDto`  
**Response (404):** Not found

### POST /roles

**Request:**
```json
{
  "roleName": "Manager"
}
```

**Response (201):** `Location: /api/v1/roles/{id}`

### PUT /roles/{id}

**Request:**
```json
{
  "roleName": "UpdatedRole"
}
```

**Response (204):** Success  
**Response (404):** Not found

### DELETE /roles/{id}

**Response (204):** Success  
**Response (404):** Not found

---

## Tags

### GET /tags

**Response (200):** Array of `TagDto`

### GET /tags/{id}

**Response (200):** `TagDto`  
**Response (404):** Not found

### POST /tags

**Request:**
```json
{
  "tagName": "frontend",
  "tagColor": "#3B82F6"
}
```

**Response (201):** `Location: /api/v1/tags/{id}`

### DELETE /tags/{id}

**Response (204):** Success  
**Response (404):** Not found

---

## Task Tags

### POST /task-tags/{taskId}/{tagId}

Adds a tag to a task.

**Response (204):** Success

### DELETE /task-tags/{taskId}/{tagId}

Removes a tag from a task.

**Response (204):** Success

### GET /task-tags/task/{taskId}

Retrieves all tags for a task.

**Response (200):** Array of `TagDto`

---

## User Roles

### POST /user-roles/{userId}/{roleId}

Assigns a role to a user.

**Response (204):** Success

### DELETE /user-roles/{userId}/{roleId}

Removes a role from a user.

**Response (204):** Success

### GET /user-roles/user/{userId}

Retrieves all roles for a user.

**Response (200):** Array of `RoleDto`

---

## Activity Logs

### GET /activity-logs/task/{taskId}

**Response (200):** Array of `ActivityLogDto`

### GET /activity-logs/user/{userId}

**Response (200):** Array of `ActivityLogDto`

---

## File Attachments

### POST /attachments

**Request:**
```json
{
  "fileName": "screenshot.png",
  "url": "https://example.com/files/screenshot.png",
  "taskId": "guid",
  "userId": "guid"
}
```

**Response (201):** `Location: /api/v1/attachments/{id}`

### GET /attachments/{id}

**Response (200):** `AttachmentDto`  
**Response (404):** Not found

### GET /attachments/task/{taskId}

**Response (200):** Array of `AttachmentDto`

### DELETE /attachments/{id}

**Response (204):** Success  
**Response (404):** Not found

---

## Enums

### TaskStatus

| Value | Description |
|-------|-------------|
| `Todo` | Task is created but not started |
| `InProgress` | Task is actively being worked on |
| `Done` | Task is completed (cannot be reopened) |
| `Cancelled` | Task was cancelled |

### TaskPriority

| Value | Description |
|-------|-------------|
| `Low` | Low priority (numeric value: 1) |
| `Medium` | Medium priority (numeric value: 2) |
| `High` | High priority (numeric value: 3) |
| `Critical` | Critical priority (numeric value: 4) |

---

## Rate Limiting

All endpoints are subject to rate limiting:
- **Token bucket**: 100 requests per minute per IP address
- **429 Too Many Requests** response when limit exceeded
- `Retry-After` header included in 429 responses

---

## Response Caching

| Endpoint | Cache Duration | Vary By |
|----------|---------------|---------|
| `GET /tasks` | 30 seconds | page, pageSize, status, priority, userId, searchTerm |
| `GET /tasks/{id}` | 30 seconds | id |
| `GET /users` | 60 seconds | — |
| `GET /users/{id}` | 60 seconds | id |
