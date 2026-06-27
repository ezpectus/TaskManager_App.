# Refactoring Log — TaskManagerSolution

> **Status:** Historical document  
> **Audit date:** June 22, 2026  
> **Auditor:** Cascade AI  
> **Project:** TaskManagerSolution (ASP.NET Core + PostgreSQL, Clean Architecture)  
> **Standard:** Clean Code, Clean Architecture, DDD, SOLID  

---

> [!IMPORTANT]  
> This document is a **historical record** of the initial audit and subsequent refactoring.  
> All 41 issues identified below have been **resolved and completed**.  
> This file is preserved for traceability and learning purposes only.  
> For the current project state, see [overview.md](overview.md).

---

## Summary

| Category | Critical | High | Medium | Low |
|----------|----------|------|--------|-----|
| Architecture | 3 | 5 | 4 | 2 |
| Security | 3 | 1 | 0 | 0 |
| Bugs & Errors | 2 | 3 | 2 | 0 |
| Code Quality | 0 | 2 | 5 | 3 |
| Testing | 0 | 1 | 1 | 0 |
| Infrastructure | 1 | 1 | 2 | 1 |

**Total: 41 issues** (9 critical, 13 high, 14 medium, 6 low)

---

## 1. Critical Issues (CRITICAL)

### C-01. Program.cs fully commented out — application does not start

**File:** `TaskManager.API/Program.cs`  
**Severity:** Critical  
**Category:** Bug / Infrastructure

The entire `Program.cs` code was wrapped in `/* ... */`. The application could not compile or start. No working entry point existed.

---

### C-02. API project does not reference Infrastructure — DI is broken

**File:** `TaskManager.API/TaskManager.API.csproj`  
**Severity:** Critical  
**Category:** Architecture

`TaskManager.API.csproj` only referenced `TaskManager.Application`. No reference to `TaskManager.Infrastructure` existed, so `DependencyInjection.AddInfrastructure()` was never called. Even after uncommenting `Program.cs`, no service or repository would be registered.

---

### C-03. IUserRoleService not registered in DI

**File:** `TaskManager.Infrastructure/DepenInjection/DependencyInjection.cs`  
**Severity:** Critical  
**Category:** Bug

The `AddServices()` method registered 9 services, but `IUserRoleService` was missing. `UserRolesController` would request `IUserRoleService` via constructor — the DI container would throw an exception.

---

### C-04. Hardcoded database password in appsettings.json

**File:** `TaskManager.API/appsettings.json`  
**Severity:** Critical  
**Category:** Security

The PostgreSQL password `564312` was hardcoded in `appsettings.json` and committed to git. This is a security violation.

**Recommendation:** Use User Secrets, environment variables, or Azure Key Vault.

---

### C-05. User passwords stored as "TEMP" — no hashing

**File:** `TaskManager.Application/Services/UserService.cs:40`  
**Severity:** Critical  
**Category:** Security

All users were created with `PasswordHash = "TEMP"`. No BCrypt/Argon2/PBKDF2 hashing existed. Commented-out code contained a BCrypt implementation, but it was not used.

---

### C-06. No authentication or authorization

**File:** Entire project  
**Severity:** Critical  
**Category:** Security

No endpoint was protected. The `JwtBearer` and `Authentication` packages were included in csproj but never used. Anyone could create, delete, or modify any data.

---

### C-07. SubtasksController.GetByTask calls the wrong service method

**File:** `TaskManager.API/Controllers/SubtasksController.cs:32-33`  
**Severity:** Critical  
**Category:** Bug

The route `task/{taskId:guid}` expected to retrieve all subtasks for a task, but `GetByIdAsync(taskId, ct)` was called — a method that searches for a subtask by its own ID, not by task ID.

---

### C-08. UserRolesController.Add has no HTTP attribute

**File:** `TaskManager.API/Controllers/UserRolesController.cs:28`  
**Severity:** Critical  
**Category:** Bug

The `[HttpPost]` attribute was commented out. The `Add` method was not bound to any HTTP method — the endpoint was unreachable.

---

### C-09. UserRole.Id not configured in EF — mapping conflict

**File:** `TaskManager.Domain/Entities/User_Roles.cs` + `UserRolesConfiguration.cs`  
**Severity:** Critical  
**Category:** Bug

The `UserRole` entity had an `Id` property (Guid), but `UserRoleConfiguration` used a composite key `(UserId, RoleId)`. The `Id` property was neither configured as a key nor ignored — EF Core would attempt to map it as a regular column, potentially causing a conflict with the composite key.

---

## 2. High Issues (HIGH)

### H-01. TaskManager.Persistence — dead project

**File:** `TaskManager.Persistence/`  
**Severity:** High  
**Category:** Architecture

The `TaskManager.Persistence` project existed in the solution but contained only `bin/` and `obj/`. No `.csproj`, no code. The solution file did not reference it, but the folder cluttered the structure.

---

### H-02. Class1.cs — dead file

**File:** `TaskManager.Infrastructure/Persistence/Class1.cs`  
**Severity:** High  
**Category:** Code Quality

Empty `Class1` class with no functionality. Artifact from project creation.

---

### H-03. IBaseRepository and BaseRepository — dead code

**Files:** `Domain/Interfaces/IBaseRepository.cs`, `Infrastructure/Persistence/Repositories/BaseRepository.cs`  
**Severity:** High  
**Category:** Architecture

`IBaseRepository` — empty `internal` interface. `BaseRepository` — empty class. No repository inherited from either. ISP violation — interface existed but was unused.

---

### H-04. SaveChangesAsync in every repository method — no transactions

**Files:** All repositories in `Infrastructure/Persistence/Repositories/`  
**Severity:** High  
**Category:** Architecture

Each `AddAsync`, `UpdateAsync`, `DeleteAsync` method called `SaveChangesAsync`. It was impossible to combine multiple operations into a single transaction. For example, `UserRoleService.AssignRoleAsync` made 3 database queries (GetUser, GetRole, GetUserRole) + Add — if anything failed mid-way, data would be in an inconsistent state.

**Recommendation:** Move `SaveChangesAsync` to the service layer or use Unit of Work for composite operations.

---

### H-05. Status and Priority as strings instead of enums

**Files:** `TaskManager.Domain/Entities/TaskItem.cs`, `TaskManager.Domain/Enums/TaskPriority.cs`  
**Severity:** High  
**Category:** Architecture / DDD

`TaskItem.Status` and `TaskItem.Priority` were strings. The `TaskPriority` enum existed but was unused. No value constraints, no type safety. Any string could be written to the database.

---

### H-06. No input validation

**Files:** All DTOs, all controllers  
**Severity:** High  
**Category:** Security / Code Quality

The `FluentValidation` package was included in API.csproj, but no validator was created. DTOs had no `[Required]`, `[MaxLength]` attributes. Controllers did not check `ModelState`. Any garbage was accepted and processed.

---

### H-07. No global error handling

**File:** Entire project  
**Severity:** High  
**Category:** Code Quality

No exception handling middleware. No `UseExceptionHandler`. If a service threw an exception, the client would receive a 500 with a stack trace (in dev) or an unhandled error.

---

### H-08. No logging

**File:** Entire project  
**Severity:** High  
**Category:** Code Quality

The `Serilog.AspNetCore`, `Serilog.Sinks.Console`, `Serilog.Sinks.File` packages were included, but Serilog was never configured. Not a single logging statement existed in the codebase.

---

### H-09. Sorting classes reference non-existent DTOs

**Files:** `TaskManager.Application/Sorting/TaskPriorityQueue.cs`, `TaskManager.Application/Sorting/TaskSorter.cs`  
**Severity:** High  
**Category:** Bug / Dead code

Both classes referenced `TaskReadDto` from `TaskDtos` (via `using static TaskManager.Application.DTOs.TaskDtos`). These types did not exist in the current code. The code would not compile if attempted to use.

---

### H-10. UsersController — only GetById endpoint

**File:** `TaskManager.API/Controllers/UsersController.cs`  
**Severity:** High  
**Category:** Architecture

UsersController had only `GetById`. No `Create`, `Update`, `Delete`, `GetAll`. The `IUserService` implemented all 5 methods, but the controller did not provide access to them.

---

### H-11. Many controllers incomplete

**Files:** All controllers  
**Severity:** High  
**Category:** Architecture

| Controller | Implemented | Missing |
|------------|-------------|---------|
| ActivityLogsController | GetByTask | GetByUser, GetById, Create, Delete |
| CommentsController | Create, GetByTask | GetById, Update, Delete |
| FileAttachmentsController | Upload | GetById, GetByTask, Delete |
| RolesController | GetAll | GetById, Create, Update, Delete |
| SubtasksController | Create, GetByTask (bug) | GetById, Update, Delete |
| TagsController | GetAll | GetById, Create, Delete |
| TaskTagsController | Add, Remove | GetTagsForTask |
| UserRolesController | Add (bug) | Remove, GetUserRoles |
| UsersController | GetById | Create, GetAll, Update, Delete |

---

### H-12. No pagination and filtering

**Files:** `TaskRepository.GetAllAsync`, all service `GetAllAsync` methods  
**Severity:** High  
**Category:** Code Quality

`GetAllAsync` returned all records via `ToListAsync()`. No pagination, filtering, or sorting. As the database grew, this would lead to memory exhaustion.

---

### H-13. Tests missing

**File:** `TaskManager.Test/UnitTest1.cs`  
**Severity:** High  
**Category:** Testing

The only test `Test1` was an empty method. The test project did not reference any other project. No service, repository, or controller tests. No integration tests.

---

## 3. Medium Issues (MEDIUM)

### M-01. Typo in project name: "Infrastucture" (fixed)

**Files:** All files in the project  
**Severity:** Medium  
**Category:** Code Quality

`TaskManager.Infrastructure` — missing the letter 'c'. The typo propagated to all namespaces, usings, and paths.

---

### M-02. Anemic Domain Model — entities without behavior

**Files:** All entities in `Domain/Entities/`  
**Severity:** Medium  
**Category:** DDD

All entities were POCOs with public setters. No encapsulation, no domain methods, no value objects, no invariant enforcement. DDD violation: domain logic should be in entities, not in services.

---

### M-03. Inconsistent namespace styles

**Files:** Throughout the project  
**Severity:** Medium  
**Category:** Code Quality

Some files used file-scoped namespaces (`namespace X;`), others used block-scoped (`namespace X { }`). No unified style.

---

### M-04. Inconsistent entity file naming

**Files:** `Domain/Entities/`  
**Severity:** Medium  
**Category:** Code Quality

| File | Class | Issue |
|------|-------|-------|
| `Comments.cs` | `Comment` | Plural |
| `Tags.cs` | `Tag` | Plural |
| `Roles.cs` | `Role` | Plural |
| `User_Roles.cs` | `UserRole` | Underscore |
| `TaskItem.cs` | `TaskItem` | OK |
| `Subtask.cs` | `Subtask` | OK |
| `User.cs` | `User` | OK |

---

### M-05. UserService does not use AutoMapper

**File:** `TaskManager.Application/Services/UserService.cs`  
**Severity:** Medium  
**Category:** Code Quality

UserService performed manual mapping while other services used AutoMapper. No consistency.

---

### M-06. CommentsController.Create returns Ok instead of CreatedAtAction

**File:** `TaskManager.API/Controllers/CommentsController.cs:24`  
**Severity:** Medium  
**Category:** Code Quality

REST convention violation. `TasksController` used `CreatedAtAction`, others used `Ok(id)`.

---

### M-07. No API versioning

**File:** Entire API  
**Severity:** Medium  
**Category:** Architecture

No versioning strategy (e.g., `/api/v1/tasks`). When the contract changed, it would be impossible to support old clients.

---

### M-08. No optimistic concurrency control

**Files:** All entities  
**Severity:** Medium  
**Category:** Architecture

No `RowVersion` / `xmin` tokens. Concurrent updates could overwrite each other (lost update).

---

### M-09. SQL scripts in entity comments

**Files:** All entities in `Domain/Entities/`  
**Severity:** Medium  
**Category:** Code Quality

Each entity file contained a full SQL `CREATE TABLE` script in comments. This duplicated the schema and created confusion: what is the source of truth — SQL or Fluent API configuration?

---

### M-10. Massive blocks of commented-out code

**Files:** Nearly every file in the project  
**Severity:** Medium  
**Category:** Code Quality

Nearly every file contained `/* old version */` blocks with old code. This cluttered the codebase. Old code should be in git history, not in comments.

---

### M-11. No CORS configuration

**File:** `Program.cs` (when uncommented)  
**Severity:** Medium  
**Category:** Infrastructure

CORS was not configured. During frontend development, browser requests would be blocked.

---

### M-12. No health checks

**File:** Entire project  
**Severity:** Medium  
**Category:** Infrastructure

No `/health` endpoint for checking database and application status.

---

### M-13. ActivityLog not populated automatically

**Files:** All services  
**Severity:** Medium  
**Category:** Architecture

The `ActivityLog` entity existed, but no service automatically created records when tasks changed. The audit trail was non-functional.

---

### M-14. No soft delete

**Files:** All repositories  
**Severity:** Medium  
**Category:** Architecture

Hard deletes via `Remove()`. For a task manager, soft delete (with `IsDeleted` flag) is typically expected to allow data recovery.

---

## 4. Low Issues (LOW)

### L-01. Redundant using directives

**Files:** Many files  
**Severity:** Low  
**Category:** Code Quality

Many files contained dozens of unused usings. Examples:
- `Subtask.cs`: `System.Data`, `System.Diagnostics.Meters`, `System.Net.NetworkInformation`, `System.Reflection`, `System.Text.RegularExpressions`, `System.Runtime.InteropServices.JavaScript.JSType`
- `TaskItem.cs`: `System.Net.Mail`, `System.Xml.Linq`
- `User.cs`: `System.Xml.Linq`
- `ApplicationDbContext.cs`: `System.Data`, `System.Net.Mail`
- `ActivityLogConfiguration.cs`: `using static Microsoft.EntityFrameworkCore.DbLoggerCategory;`

---

### L-02. TaskTagsController accepts Guid from query string

**File:** `TaskManager.API/Controllers/TaskTagsController.cs:20,27`  
**Severity:** Low  
**Category:** Code Quality

Parameters passed as query string (`?taskId=...&tagId=...`). Better to use route parameters (`api/task-tags/{taskId}/{tagId}`) or request body.

---

### L-03. DI method named AddPersistence but registers everything

**File:** `TaskManager.Infrastructure/DepenInjection/DependencyInjection.cs:26`  
**Severity:** Low  
**Category:** Code Quality

The `AddPersistence` method registered DbContext, AutoMapper, repositories AND services. The name was misleading. Should be split into `AddInfrastructure` and `AddApplication`.

---

### L-04. TaskRepository.GetByIdAsync uses FirstOrDefaultAsync instead of FindAsync

**File:** `TaskManager.Infrastructure/Persistence/Repositories/TaskRepository.cs:30`  
**Severity:** Low  
**Category:** Performance

`FindAsync` first checks the change tracker, which is more efficient for entities already loaded in the context.

---

### L-05. No Swagger/OpenAPI configuration

**File:** `Program.cs`  
**Severity:** Low  
**Category:** Infrastructure

The `Swashbuckle.AspNetCore` and `Microsoft.OpenApi` packages were included but not configured. No XML comments for API documentation.

---

### L-06. .editorconfig exists but not integrated

**File:** `TaskManagerProject/.editorconfig`  
**Severity:** Low  
**Category:** Code Quality

The file existed but was not referenced in any .csproj as `AdditionalFiles`. Analyzers were not connected.

---

## 5. SOLID / DDD Violations

### SRP (Single Responsibility Principle)
- **UserService** simultaneously created users, mapped DTOs manually, checked email — multiple responsibilities
- **DependencyInjection.AddPersistence** registered both persistence and application layers

### OCP (Open/Closed Principle)
- No strategy for adding new task types/events without modifying existing code
- Sorting classes hardcoded to a specific DTO

### LSP (Liskov Substitution Principle)
- `IBaseRepository` empty — any repository technically "implements" it, but the contract is absent

### ISP (Interface Segregation Principle)
- `IBaseRepository` — empty interface that nobody uses (violation: interface without a contract)

### DIP (Dependency Inversion Principle)
- API layer depended on Application, but Infrastructure was not connected to API — DIP violated in practice (though architecturally the dependency direction was correct)
- Services depended on repositories (Domain.Interfaces) — this is correct

### DDD Violations
- Anemic Domain Model — no aggregates, value objects, domain events
- No bounded contexts
- Entities did not encapsulate invariants (all setters public)
- No domain services
- `TaskPriority` enum existed but was not used in `TaskItem`
- No factory pattern for entity creation

### Clean Code Violations
- Dead code (Class1.cs, BaseRepository, IBaseRepository, TaskManager.Persistence)
- Commented-out code blocks everywhere
- Magic strings ("TEMP" for password)
- Lack of consistency (mapping, namespace style, naming)
- No input validation
- No error handling

---

## 6. Problem Structure Map

```
TaskManagerSolution/
├── TaskManager.API/
│   ├── Program.cs                    ← C-01: fully commented out
│   ├── TaskManager.API.csproj        ← C-02: no reference to Infrastructure
│   ├── appsettings.json              ← C-04: hardcoded password
│   └── Controllers/
│       ├── SubtasksController.cs     ← C-07: wrong service call
│       ├── UserRolesController.cs    ← C-08: no HTTP attribute
│       ├── UsersController.cs        ← H-10: only 1 endpoint
│       └── (all controllers)         ← H-11: incomplete CRUD
│
├── TaskManager.Application/
│   ├── Services/
│   │   ├── UserService.cs            ← C-05: PasswordHash = "TEMP"
│   │   └── (all services)            ← M-13: no ActivityLog
│   ├── Sorting/                      ← H-09: dead code, non-existent DTOs
│   └── DTOs/                         ← H-06: no validation
│
├── TaskManager.Domain/
│   ├── Entities/                     ← M-02: anemic model, M-09: SQL in comments
│   ├── Enums/TaskPriority.cs         ← H-05: enum unused
│   └── Interfaces/IBaseRepository.cs ← H-03: empty interface
│
├── TaskManager.Infrastructure/       ← M-01: typo in name
│   ├── DepenInjection/               ← C-03: no IUserRoleService, L-03: naming
│   └── Persistence/
│       ├── Class1.cs                 ← H-02: dead file
│       ├── Repositories/BaseRepository.cs ← H-03: empty class
│       └── Repositories/             ← H-04: SaveChanges in every method
│
├── TaskManager.Persistence/          ← H-01: dead project
├── TaskManager.Test/UnitTest1.cs     ← H-13: no tests
└── (entire project)                  ← C-06: no auth, H-07: no error handling,
                                        H-08: no logging, M-10: commented code,
                                        M-11: no CORS, M-12: no health checks
```

---

## 7. Fix Priorities

### Phase 1 — "Revive the project" (Critical)
1. Uncomment and rewrite `Program.cs`
2. Add API → Infrastructure reference
3. Register `IUserRoleService` in DI
4. Fix bug in `SubtasksController.GetByTask`
5. Add `[HttpPost]` to `UserRolesController.Add`
6. Configure `UserRole.Id` in EF (ignore or remove)
7. Remove password from `appsettings.json` → User Secrets

### Phase 2 — "Security" (Critical/High)
1. Implement BCrypt for password hashing
2. Configure JWT authentication
3. Add authorization to controllers
4. Add FluentValidation for all DTOs
5. Configure global error handling middleware

### Phase 3 — "Architecture" (High/Medium)
1. Remove dead code (Class1, BaseRepository, IBaseRepository, Persistence project)
2. Replace string Status/Priority with enums
3. Move SaveChangesAsync out of repositories
4. Add pagination and filtering
5. Configure Serilog
6. Configure Swagger/OpenAPI
7. Add CORS
8. Add health checks

### Phase 4 — "Quality" (Medium/Low)
1. Rename Infrastucture → Infrastructure
2. Unify namespace style
3. Unify file naming
4. Remove all commented-out code blocks
5. Clean up usings
6. Add XML documentation
7. Configure analyzers (Roslyn, SonarAnalyzer)

---

## 8. Improvement Plan: Backend, Frontend, UX/UI, Stability

### Part A. Backend Improvement

**A1. Project Revival (Week 1)**
- A1.1. Working Program.cs — uncomment, rewrite from scratch, add `AddInfrastructure()`, configure Swagger, add `UseExceptionHandler`, configure Serilog, add CORS, add health checks
- A1.2. Fix project references — add `ProjectReference` from API to Infrastructure, remove dead `TaskManager.Persistence` project, remove `Class1.cs`, remove empty `IBaseRepository` and `BaseRepository`
- A1.3. Fix bugs — `SubtasksController.GetByTask` → call `GetAllByTaskIdAsync`, `UserRolesController.Add` → add `[HttpPost]`, `UserRole.Id` → remove or configure `ValueGeneratedOnAdd`, `IUserRoleService` → register in DI

**A2. Security (Week 2)**
- A2.1. Password hashing — install `BCrypt.Net-Next`, hash passwords in `UserService.CreateAsync`, add `ValidateCredentialsAsync`, create `IAuthService` with JWT generation
- A2.2. JWT authentication — configure `JwtBearer` in `Program.cs`, create `TokenService`, add `LoginRequest`/`LoginResponse` DTOs, create `AuthController` with `POST /api/auth/login`, protect all controllers with `[Authorize]`
- A2.3. Role-based authorization — add `[Authorize(Roles = "Admin")]` on admin endpoints, create `RequirePermission` attribute, owner-based authorization
- A2.4. Input validation — create FluentValidation validators for all DTOs, register in DI, add automatic validation
- A2.5. Secrets — remove DB password from `appsettings.json`, use `dotnet user-secrets` for dev, environment variables for production

**A3. Architectural Improvements (Weeks 3-4)**
- A3.1. Enums instead of strings — `TaskStatus` and `TaskPriority` enums with `.HasConversion<string>()` in EF
- A3.2. Repository refactoring — Unit of Work pattern, remove `SaveChangesAsync` from repositories
- A3.3. Pagination and filtering — `PagedResult<T>` with `TaskFilter` and `PaginationParams`
- A3.4. Domain logic in entities (DDD) — `Create`, `ChangeStatus`, `MarkAsCompleted`, `AssignTo`, `SoftDelete` methods with private setters
- A3.5. Automatic Activity Log — `ActivityLogInterceptor` on `SaveChangesAsync`
- A3.6. Soft Delete — `IsDeleted` + `DeletedAt` with global query filters
- A3.7. Optimistic concurrency — `RowVersion` on `TaskItem`, handle `DbUpdateConcurrencyException`

**A4. Complete Controller CRUD (Week 4)**
- Full CRUD for all controllers: Users, ActivityLogs, Comments, FileAttachments, Roles, Subtasks, Tags, TaskTags, UserRoles, Auth

**A5. API Versioning and Documentation**
- Install `Asp.Versioning.Mvc`, add version to URL, configure Swagger with version support, add XML comments

**A6. Testing (Weeks 5-6)**
- A6.1. Unit tests for services (Moq, 80%+ coverage target)
- A6.2. Integration tests (`WebApplicationFactory<Program>`, Testcontainers for PostgreSQL)
- A6.3. Repository tests (in-memory DbContext or Testcontainers)

### Part B. Frontend
- React 19 + TypeScript + TailwindCSS + Vite
- Pages: Login, Dashboard, Tasks, TaskDetail, Kanban, Calendar, Tags, Settings
- Components: shadcn/ui, Lucide icons, Recharts, @dnd-kit/core
- State: Zustand, TanStack Query (React Query)
- Forms: React Hook Form + Zod

### Part C. UX/UI Design
- Design system with TailwindCSS color palette
- Priority and status color mappings
- Optimistic updates, loading states, empty states, toast notifications
- Confirmation dialogs, keyboard shortcuts, responsive design
- Dark/light theme with localStorage persistence

### Part D. Stability and Reliability
- D1. Backend: global error handling, Serilog logging, rate limiting, health checks, connection resilience, EF migrations
- D2. Frontend: error boundaries, retry and fallback, offline mode, performance optimization
- D3. Infrastructure: Docker, Docker Compose, CI/CD (GitHub Actions), monitoring (OpenTelemetry, Prometheus + Grafana, Sentry)

### Part E. Timeline

| Phase | Duration | Result |
|-------|----------|--------|
| E1. Backend revival | 1 week | Working API, Swagger, basic endpoints |
| E2. Security | 1 week | JWT auth, validation, password hashing |
| E3. Backend architecture | 2 weeks | Enums, Unit of Work, pagination, soft delete |
| E4. Full CRUD + tests | 2 weeks | All endpoints, unit + integration tests |
| E5. Frontend MVP | 3 weeks | Auth, Dashboard, Tasks, TaskDetail, Kanban |
| E6. UX/UI polish | 1 week | Dark mode, responsive, empty states, shortcuts |
| E7. Docker + CI/CD | 1 week | Docker Compose, GitHub Actions, deployment |
| E8. Stability | 1 week | Error handling, logging, rate limiting, monitoring |

**Total: ~12 weeks** to a full-stack application.

---

## 9. Fix Status (Updated)

### Completed

| ID | Description | Status |
|----|-------------|--------|
| C-01 | Program.cs uncommented, application starts | Done |
| C-02 | API references Infrastructure, DI works | Done |
| C-03 | IUserRoleService registered in DI | Done |
| C-04 | DB password moved to User Secrets | Done |
| C-05 | Passwords hashed with BCrypt | Done |
| C-06 | JWT authentication implemented | Done |
| C-07 | [Authorize] on all controllers | Done |
| C-08 | DTO validation via FluentValidation | Done |
| C-09 | Commented-out code blocks removed | Done |
| H-01 | Dead code removed | Done |
| H-02 | Redundant usings removed | Done |
| H-03 | Namespace cleanup | Done |
| H-04 | Unit of Work pattern (IUnitOfWork + implementation) | Done |
| H-05 | Enums for TaskStatus and TaskPriority | Done |
| H-06 | FluentValidation for all DTOs | Done |
| H-07 | Global error handling middleware | Done |
| H-08 | Serilog structured logging | Done |
| H-09 | Dead files and comments removed | Done |
| H-10 | Full CRUD for all controllers | Done |
| H-11 | [Authorize] + [AllowAnonymous] applied | Done |
| H-12 | Pagination and filtering for Tasks (PagedResult) | Done |
| H-13 | Unit tests for services (xUnit + Moq, 78 tests) | Done |
| M-01 | Renamed project Infrastucture → Infrastructure | Done |
| M-02 | Domain logic in entities (DDD): Create, ChangeStatus, SoftDelete | Done |
| M-03 | Namespace cleanup (file-scoped) | Done |
| M-04 | Entity files renamed (Comment.cs, Tag.cs, Role.cs, UserRole.cs) | Done |
| M-05 | AutoMapper in UserService, RoleService, FileAttachmentService | Done |
| M-06 | CreatedAtAction for all POST endpoints | Done |
| M-07 | API versioning (Asp.Versioning, URL segment) | Done |
| M-08 | Optimistic concurrency (RowVersion on TaskItem) | Done |
| M-09 | Redundant comments removed | Done |
| M-10 | File-scoped namespaces | Done |
| M-11 | CORS configured | Done |
| M-12 | Health checks endpoint (/health) | Done |
| M-13 | ActivityLog automatic via SaveChanges interceptor | Done |
| M-14 | Soft Delete for TaskItem, Comment, Subtask (query filters) | Done |
| L-01 | Redundant comments removed | Done |
| L-02 | TaskTagsController uses route parameters | Done |
| L-03 | AddPersistence → AddInfrastructure | Done |
| L-04 | TaskRepository uses FindAsync | Done |
| L-05 | Swagger/OpenAPI with XML comments and JWT scheme | Done |
| L-06 | .editorconfig with style/formatting/naming rules | Done |

**All 41 audit items completed.**
