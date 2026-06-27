# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.8.0] — 2026-06-27

### Added
- **Registration page** (`RegisterPage.tsx`) with form validation (username, email, password, confirm password)
- `authService.register()` method for `POST /api/v1/users` public endpoint
- `/register` route in `App.tsx`
- Demo credentials info box on LoginPage (admin/demo accounts)
- "Sign up" / "Sign in" cross-links between Login and Register pages
- **Relative deadline utility** (`utils/deadline.ts`): "3 days left", "Overdue by 2 days", "Due today", "Due tomorrow"
- **Jira-style quick filters** on Dashboard: Overdue (with count), Due Today (with count), High Priority (with count)
- **Sort dropdown** on Dashboard: by deadline, priority, title (A-Z), or creation date (newest first)
- **Priority icons** (arrow up / minus / arrow down) across Dashboard, Kanban, and Task Detail — Jira style
- **Task counter badge** in Dashboard ("filtered / total") and Kanban page titles — Jira style
- **Deadline editing** via date picker in TaskDetailPage edit mode
- **Deadline display** on Profile page recent tasks list
- **Keyboard shortcuts overlay** (`KeyboardShortcutsOverlay.tsx`) — press `?` anywhere to toggle — Obsidian style
- Keyboard icon button in Layout nav bar
- UX Inspiration table in `README.md` documenting feature inspiration sources (Google Calendar, Jira, Obsidian, Trello)
- Code Sources & References section in `README.md` documenting architecture patterns, frontend/backend references, and database migration guide
- 18 new unit tests (60 → 78 total):
  - `Create_Should_Set_Deadline_To_MaxValue_When_Not_Provided`
  - `Create_Should_Set_Deadline_When_Provided`
  - `UpdateDetails_Should_Update_Deadline_When_Provided`
  - `UpdateDetails_Should_Preserve_Deadline_When_Not_Provided`
  - `UpdateDetails_Should_Update_UpdatedAt`
  - `Create_With_PastDeadline_Should_Store_Correctly`
  - `Create_With_FutureDeadline_Should_Store_Correctly`
  - `UpdateDetails_With_NullDeadline_Should_Keep_Existing`
  - `ChangeStatus_Should_Update_UpdatedAt`
  - `AssignTo_Should_Set_UserId`
  - `Touch_Should_Update_UpdatedAt`
  - `UpdateAsync_Should_UpdateDeadline_When_DeadlineProvided`
  - `UpdateAsync_Should_PreserveDeadline_When_DeadlineNotProvided`
  - `CreateAsync_Should_MapDeadline_From_Dto`
  - `UpdateAsync_Should_ReturnFalse_When_TaskNotFound`
  - `DeleteAsync_Should_ReturnFalse_When_TaskNotFound`
  - `AssignAsync_Should_ReturnFalse_When_TaskNotFound`
  - `AssignAsync_Should_ReturnFalse_When_UserNotFound`
- **Smart Priority Algorithm** (`utils/deadline.ts`): urgency × importance scoring — Eisenhower Matrix inspired
- **Database indexes**: `Deadline`, `Status`, `Priority`, `UserId`, composite `(Status, Priority)` on `TaskItem`
- **AsNoTracking** on all read-only task queries for reduced memory allocation
- **Include(User)** on task queries to prevent N+1 when mapping `Username` to `TaskDto`
- Performance Optimizations section in `README.md`

### Changed
- `UpdateTaskRequest` type now includes optional `deadline` field
- `LoginPage.tsx` — added `Link` import from react-router-dom, `Info` icon from lucide-react
- Dashboard empty state message now accounts for active filters
- `ProfilePage.tsx` password validation fixed from 6 to 8 characters (matches backend FluentValidation rule)
- `README.md` frontend features section expanded with all new features

### Fixed
- ProfilePage password validation mismatch (frontend: 6 chars, backend: 8 chars) — unified to 8

---

## [1.7.0] — 2026-06-27

### Added
- Architecture Decision Records (ADRs) in `docs/adr/` (6 records: PostgreSQL, Clean Architecture, BCrypt, AutoMapper, Serilog, FluentValidation)
- `CHANGELOG.md` — version history
- `CONTRIBUTING.md` — contribution guidelines
- `docs/api.md` — detailed API endpoint reference with request/response schemas
- Mermaid diagrams in `README.md` and `docs/overview.md`:
  - Clean Architecture layer diagram
  - Entity Relationship (ER) diagram
  - Authentication flow sequence diagram
  - Request flow sequence diagram
- Technology choices & rationale table in `docs/overview.md`
- API endpoints summary table in `README.md`
- Docker quick start section in `README.md`
- Testing section in `README.md` with commands and coverage summary
- Seed credentials table in `README.md`
- Detailed project structure tree in `README.md` with per-directory descriptions

### Changed
- Rewrote `README.md` from 165 lines to ~495 lines with full architecture overview, diagrams, and documentation links
- Rewrote `docs/overview.md` with Mermaid diagrams, layer table, request flow diagram, and technology rationale
- Translated `docs/audit.md` (Russian) to English and renamed to `docs/refactoring-log.md` — marked as historical document
- Unified all documentation to English (C2 technical writing style)
- Updated test count from "45+" to "60" across all docs
- Updated AutoMapper version references from 15.1.0 to 15.1.3

### Removed
- `docs/audit.md` — superseded by `docs/refactoring-log.md`
- `audit.md` entry from `.gitignore` (file moved to `docs/` and renamed)

---

## [1.6.0] — 2026-06-22

### Added
- Frontend: Analytics page with task statistics and progress bars
- Frontend: CSV export for tasks
- Frontend: Profile page with edit profile and change password functionality
- Frontend: Dark mode with CSS variables and localStorage persistence
- Frontend: Toast notifications for all CRUD operations (ToastContext)
- Frontend: ErrorBoundary component
- Frontend: ConfirmDialog component
- Frontend: LoadingSpinner component
- Frontend: Skeleton loaders for Dashboard and Kanban
- Frontend: Empty states with calls to action
- Frontend: Debounced search (useDebounce hook)
- Frontend: Keyboard shortcuts (useKeyboardShortcuts hook)
- Frontend: `.env.example` for API base URL configuration
- DevOps: `Dockerfile.backend` (multi-stage .NET 9 build)
- DevOps: `Dockerfile.frontend` (Node 20 + nginx)
- DevOps: `docker-compose.yml` (PostgreSQL + backend + frontend)
- DevOps: `nginx.conf` (SPA routing + API proxy)
- DevOps: `start.sh` for Linux one-click startup

### Changed
- Frontend: Upgraded from React 18 to React 19
- Frontend: Added `react-markdown` dependency for markdown rendering in task descriptions
- CI/CD: GitHub Actions workflow now includes frontend type check + build

### Fixed
- AutoMapper 15.x `MapperConfiguration` constructor requires `ILoggerFactory` — added `NullLoggerFactory.Instance` in test project
- `TaskStatus` ambiguous reference between `Domain.Enums.TaskStatus` and `System.Threading.Tasks.TaskStatus` — fully qualified in all test files
- `PagedResult<>` missing namespace reference in `TasksControllerTests.cs` — added `using TaskManager.Application.DTOs.Common`
- FluentValidation 12.x deprecated `AddFluentValidation()` API — migrated to `AddFluentValidationAutoValidation()` + `AddFluentValidationClientsideAdapters()`
- `AddApiExplorer` extension method missing — added `Asp.Versioning.Mvc.ApiExplorer` package

---

## [1.5.0] — 2026-06-15

### Added
- Rate limiting (token bucket, 100 req/min per IP) on TasksController and UsersController
- Response caching (30s on tasks, 60s on users) with VaryByQueryKeys
- `ActivityLogInterceptor` — automatic activity logging on SaveChangesAsync
- DB seeding on startup (DbSeeder): admin/demo users, roles, 6 sample tasks
- `Touch()` method on TaskItem entity for internal UpdatedAt management
- Health checks endpoint (`/health`)

### Changed
- Replaced direct `UpdatedAt` assignments with `Touch()` method calls in TaskService
- ActivityLogInterceptor casts `DbContext` to `ApplicationDbContext` to access ActivityLogs DbSet

### Fixed
- Infrastructure project folder typo: "Infrastucture" → "Infrastructure"
- AutoMapper version conflict across projects — unified to 15.1.0 (later 15.1.3)
- `Microsoft.OpenApi.Models` namespace conflict — removed explicit `Microsoft.OpenApi` package, let Swashbuckle bring compatible version
- Missing `UpdateUserRequest` and `ChangePasswordRequest` DTOs in UsersController — added `using TaskManager.Application.DTOs.Auth`
- All 12 FluentValidation validators missing DTO namespace references — added appropriate `using` directives

---

## [1.4.0] — 2026-06-10

### Added
- Frontend: Kanban board with drag & drop between status columns
- Frontend: Task detail page with inline editing, subtasks, comments
- Frontend: Dashboard with task grid, search, filter chips, table/list toggle
- Frontend: Login page with theme toggle
- Frontend: NotFound (404) page
- Frontend: Layout component with navigation sidebar
- Frontend: API client (`apiFetch`) with auto-auth headers and 401 redirect
- Frontend: AuthContext with login/logout, protected routes
- Frontend: Services: taskService, authService, subtaskService, commentService, userService
- Frontend: Hooks: useTasks, useTask, useDebounce

---

## [1.3.0] — 2026-06-05

### Added
- JWT authentication with refresh tokens
- BCrypt password hashing (BCrypt.Net-Next)
- AuthController with login and refresh endpoints
- TokenService for JWT generation and validation
- Global exception handling middleware
- Serilog structured logging (Console + File sinks)
- Swagger/OpenAPI with XML comments and JWT Bearer scheme
- CORS configuration
- API versioning via URL segment (`api/v1/...`)

### Changed
- All controllers protected with `[Authorize]` attribute
- Password hashing migrated from "TEMP" placeholder to BCrypt

---

## [1.2.0] — 2026-06-01

### Added
- Unit of Work pattern (`IUnitOfWork` + implementation)
- Soft delete with query filters (TaskItem, Comment, Subtask)
- Optimistic concurrency control (RowVersion on TaskItem)
- Pagination and filtering for tasks (`PagedResult<T>`)
- Task assignment to users
- Activity log endpoint
- FluentValidation validators for all DTOs
- AutoMapper MappingProfile
- DB seeding infrastructure

### Changed
- Repositories no longer call `SaveChangesAsync` internally — moved to Unit of Work
- Entity properties changed from public setters to private setters with domain methods
- Status and Priority changed from strings to enums (`TaskStatus`, `TaskPriority`)
- Entity file naming unified (Comments.cs → Comment.cs, Tags.cs → Tag.cs, etc.)

### Removed
- Dead code: Class1.cs, BaseRepository, IBaseRepository, TaskManager.Persistence project
- All commented-out code blocks across the codebase
- SQL scripts from entity comments

---

## [1.1.0] — 2025-12-15

### Added
- Full CRUD for all controllers (Users, Roles, Tags, Subtasks, Comments, FileAttachments, ActivityLogs, TaskTags, UserRoles)
- `CreatedAtAction` for all POST endpoints (REST convention)
- `.editorconfig` with style, formatting, and naming rules
- File-scoped namespaces across the entire codebase

### Changed
- DI method renamed from `AddPersistence` to `AddInfrastructure`
- TaskRepository uses `FindAsync` instead of `FirstOrDefaultAsync`
- TaskTagsController uses route parameters instead of query string

---

## [1.0.0] — 2025-06-01

### Added
- Initial project structure with Clean Architecture (Domain, Application, Infrastructure, API, Test)
- Basic entities: TaskItem, User, Subtask, Comment, Tag, Role, UserRole, FileAttachment, ActivityLog, TaskTag
- EF Core with PostgreSQL, Fluent API configuration
- Basic controllers with partial CRUD
- xUnit test project (placeholder)
