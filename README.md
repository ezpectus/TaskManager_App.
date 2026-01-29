# Task Manager App

Task Manager App is a backend-focused educational project built with ASP.NET Core and PostgreSQL.

The goal of the project is to explore backend architecture, data modeling, and design patterns in a real codebase without scaffolding, generators, or shortcuts.

This repository represents a personal architectural experiment rather than a production-ready system.

## Tech Stack
- ASP.NET Core Web API
- C#
- PostgreSQL
- Entity Framework Core (Fluent API)
- Clean Architecture (simplified)

## Project Type
- Personal
- Educational / experimental
- Backend-focused

## Documentation
- [Project Overview](docs/overview.md)
- [Architecture Decisions](docs/architecture.md)
- [Roadmap](docs/roadmap.md)
  

---



**Task Manager App** :
- PostgreSQL: Tables for User, Project, Task, and Tag with proper relations, indexes, and migrations
- EF Core + Fluent API: Entity models with constraints, cascade rules, and validation logic
- ASP.NET Core API: RESTful CRUD controllers following SOLID principles and dependency injection
- JWT Authentication: Secure signup/login with role-based access control
- Task Features: Deadlines, priorities, statuses, and tag filtering
- Microservices (planned): Notifications, Analytics, and Auth separation via REST or RabbitMQ
- Frontend (later): Blazor or React with responsive UI and drag-and-drop task board

  
