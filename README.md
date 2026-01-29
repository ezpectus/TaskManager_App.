# Task Manager App

Backend-focused Task Manager built with .NET and EF Core.

## Tech Stack
- .NET
- EF Core
- PostgreSQL
- Fluent API
- Clean Architecture

## Project Structure
- `src/` — application source code
- `tests/` — unit and integration tests
- `docs/` — architecture and decisions

## Architecture
Detailed explanation available in [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md)

## Status
This project focuses on backend architecture and learning trade-offs.
Frontend, Swagger and additional tooling were not completed due to time constraints.

📄 Documentation
- Architecture overview → docs/architecture.md
- Key decisions → docs/decisions.md
- Roadmap → docs/roadmap.md

https://www.canva.com/design/DAG_thgFeFw/6kJIV1_pnsmwHWIz_R8PWQ/edit



**Task Manager App** :
- PostgreSQL: Tables for User, Project, Task, and Tag with proper relations, indexes, and migrations
- EF Core + Fluent API: Entity models with constraints, cascade rules, and validation logic
- ASP.NET Core API: RESTful CRUD controllers following SOLID principles and dependency injection
- JWT Authentication: Secure signup/login with role-based access control
- Task Features: Deadlines, priorities, statuses, and tag filtering
- Microservices (planned): Notifications, Analytics, and Auth separation via REST or RabbitMQ
- Frontend (later): Blazor or React with responsive UI and drag-and-drop task board

  
