# Task Manager App

Task Manager App is a full-stack task management application built with ASP.NET Core Web API (Clean Architecture) and React frontend.

The project was created as a practical sandbox for testing backend architecture, data modeling, and design patterns in real code, and has evolved into a complete full-stack application with authentication, CRUD operations, Kanban board, and more.

---

## Features

### Backend
- Clean Architecture (Domain, Application, Infrastructure, API, Tests)
- JWT authentication with BCrypt password hashing and refresh tokens
- Password change endpoint with current password verification
- User profile update (username, email)
- Domain-Driven Design: entity factory methods, encapsulated state transitions, soft delete
- Unit of Work pattern with `IUnitOfWork`
- EF Core with PostgreSQL, Fluent API configuration
- Soft delete with query filters (TaskItem, Comment, Subtask)
- Optimistic concurrency control (RowVersion on TaskItem)
- Pagination and filtering for tasks
- Task assignment to users
- Activity log endpoint for tracking task changes
- API versioning (URL segment: `api/v1/...`)
- Automatic activity logging via SaveChanges interceptor
- Rate limiting (token bucket, 100 req/min per IP)
- Response caching on tasks and users endpoints
- FluentValidation for all DTOs
- AutoMapper for entity-to-DTO mapping
- Serilog structured logging
- Global exception handling middleware
- Swagger/OpenAPI with XML comments and JWT Bearer scheme
- CORS and health checks configured
- DB seeding on startup (admin/demo users, roles, sample tasks)
- Unit tests (xUnit + Moq + AutoMapper): 45+ tests across services and domain entities

### Frontend
- React 19 + TypeScript + TailwindCSS + Vite
- JWT-based authentication with protected routes and refresh token support
- Dashboard with task grid, search, filter chips, and table/list view toggle
- Task detail page with inline editing, subtasks, comments, and markdown rendering
- Kanban board with drag & drop between status columns
- Analytics page with task statistics and progress bars
- CSV export for tasks
- Profile page with edit profile and change password functionality
- Dark mode with CSS variables and localStorage persistence
- Toast notifications for all CRUD operations
- Responsive layout with Lucide icons
- API proxy to backend during development

---

## Tech Stack

### Backend
- ASP.NET Core 9 Web API
- C# / .NET 9
- PostgreSQL + EF Core 9
- AutoMapper, FluentValidation, Serilog
- Asp.Versioning.Mvc (API versioning)
- xUnit + Moq (testing)

### Frontend
- React 19
- TypeScript
- TailwindCSS 3
- Vite 7
- React Router 7
- Lucide React (icons)
- react-markdown (markdown rendering)

---

## Project Structure

```
TaskManagerSolution/
├── TaskManagerProject/
│   ├── TaskManager.Domain/          # Entities, enums, interfaces
│   ├── TaskManager.Application/     # Services, DTOs, mapping, validation
│   ├── TaskManager.Infrastructure/  # EF Core, repositories, DI, interceptors
│   ├── TaskManager.API/             # Controllers, Program.cs, middleware
│   ├── TaskManager.Test/            # Unit tests (xUnit + Moq)
│   ├── frontend/                    # React frontend (Vite + TailwindCSS)
│   └── .editorconfig                # Code style rules
├── docs/
│   ├── overview.md                  # Project overview
│   ├── architecture.md              # Architecture decisions
│   ├── roadmap.md                   # Development roadmap
│   ├── future.md                    # Future plans
│   └── ideas.md                     # Extension ideas with detailed features
├── audit.md                         # Full audit with status
├── start.bat                        # One-click startup script
└── README.md
```

---

## Getting Started

### Prerequisites
- .NET 9 SDK
- PostgreSQL (running on localhost:5432)
- Node.js 20+ and npm

### Quick Start
```batch
start.bat
```
This will:
1. Restore NuGet packages
2. Build the solution
3. Start the backend API on http://localhost:5000
4. Install frontend dependencies and start Vite dev server on http://localhost:3000

### Manual Start

**Backend:**
```bash
cd TaskManagerProject
dotnet restore
dotnet build
cd TaskManager.API
dotnet run
```

**Frontend:**
```bash
cd TaskManagerProject/frontend
npm install
npm run dev
```

### Configuration
- Database connection string is in `appsettings.json` (use User Secrets for production)
- JWT secret key should be set via environment variable or User Secrets
- Frontend proxies `/api` requests to `http://localhost:5000` during development

---

## Documentation
- [Project Overview](docs/overview.md)
- [Architecture Decisions](docs/architecture.md)
- [Roadmap](docs/roadmap.md)
- [Future Plans](docs/future.md)
- [Extension Ideas](docs/ideas.md)
- [Full Audit](audit.md)

---

## Project Type
- Personal project
- Educational / experimental
- Full-stack (backend + frontend)
- Architecture-driven

---

## License
This project is for educational purposes. No license is provided.
