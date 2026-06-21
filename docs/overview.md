# Task Manager App - Project Overview

## Overview

Task Manager App is a backend-focused educational and experimental project created to practice new technologies, architectural approaches, and design patterns using ASP.NET Core.

The project was never intended to be a commercial or production-ready system. Its primary goal is to validate architectural ideas in real code and to understand how they behave during long-term solo development without scaffolding, generators, or shortcuts.

The repository is published publicly as a personal learning project and architectural experiment.

---

## Motivation

The idea for this project emerged during the summer as a deliberate decision to move from theory to practice.

The main motivations were:
- to experiment with new backend technologies in a controlled environment
- to apply architectural patterns that were previously studied only in theory
- to understand how backend architecture is built from scratch without relying on templates or auto-generated code
- to gain experience that can later be transferred to other languages and technology stacks

The focus was intentionally placed on structure, reasoning, and architectural decisions rather than development speed or feature count.

---

## Current State

### What Is Implemented

The project is now a full-stack application with both backend and frontend:

**Backend:**
- ASP.NET Core 9 Web API with Clean Architecture
- JWT authentication with BCrypt password hashing
- Domain-Driven Design: entity factory methods, encapsulated state transitions
- Unit of Work pattern with `IUnitOfWork`
- EF Core with PostgreSQL, Fluent API configuration
- Soft delete with query filters (TaskItem, Comment, Subtask)
- Optimistic concurrency control (RowVersion on TaskItem)
- Pagination and filtering for tasks
- API versioning (URL segment: `api/v1/...`)
- Automatic activity logging via SaveChanges interceptor
- FluentValidation for all DTOs
- AutoMapper for entity-to-DTO mapping
- Serilog structured logging
- Global exception handling middleware
- Swagger/OpenAPI with XML comments and JWT Bearer scheme
- CORS and health checks
- Unit tests (xUnit + Moq): 30+ tests

**Frontend:**
- React 19 + TypeScript + TailwindCSS + Vite
- JWT-based authentication with protected routes
- Dashboard with task grid, search, and create modal
- Task detail page with inline editing, subtasks, and comments
- Kanban board with 3 columns (To Do / In Progress / Done)
- Responsive layout with Lucide icons

---

## Architecture Summary

The project is built using a simplified version of Clean Architecture with selected concepts inspired by Domain-Driven Design.

### Layers

- **API** — HTTP controllers and request/response contracts
- **Application** — services and business logic
- **Domain** — domain models and core entities
- **Persistence** — EF Core mapping and PostgreSQL interaction
- **Infrastructure** — external and infrastructure-related dependencies
- **Tests** — placeholder layer for future automated tests

The core rule is simple:  
**inner layers do not depend on outer layers**.

---

## Database & EF Core Usage

The PostgreSQL schema was designed and created manually.

Entity Framework Core is not used to generate the database schema. Instead, it is used strictly as an ORM for:
- mapping existing tables
- handling data access
- defining relationships and constraints
- enforcing consistency between code and database

All configuration is performed using Fluent API.  
EF Core is treated as a tool for explicit control, not as a magic abstraction.

---

## DTO and Request Models

DTOs and request models are intentionally separated.

This separation exists because:
- client input is not the same as application data
- it provides explicit control over what the client is allowed to send
- it prevents accidental API contract changes
- it simplifies handling canceled requests and network failures

This decision emerged from practical experience rather than theoretical guidelines.

---

## Unit of Work

A custom Unit of Work abstraction (`IUnitOfWork`) is implemented in the Domain layer with a concrete implementation in Infrastructure.

- Services inject `IUnitOfWork` and call `SaveChangesAsync` after write operations
- Repositories no longer call `SaveChangesAsync` internally
- This ensures transactional consistency across multiple repository operations
- An `ActivityLogInterceptor` automatically logs entity changes on `SaveChangesAsync`

---

## Constraints & Trade-offs

The main constraints of the project were:
- solo development
- limited available time
- strong focus on backend architecture
- no requirement to finish all planned features

As a result:
- development took more than six months
- approximately 60% of planned features remain unimplemented
- parts of the architecture are intentionally over-engineered for learning purposes

---

## Planned Features (If Development Continues)

The following features are now implemented:
- ~~frontend application (React)~~ ✅
- ~~JWT-based authentication and authorization~~ ✅
- ~~Swagger / OpenAPI integration~~ ✅
- ~~Unit of Work pattern~~ ✅
- ~~Pagination and filtering~~ ✅
- ~~Soft delete~~ ✅
- ~~API versioning~~ ✅
- ~~Unit tests~~ ✅

Potential future directions:
- Notifications system (email, in-app)
- Messaging via RabbitMQ or Kafka
- Real-time updates with SignalR
- Docker containerization and CI/CD
- Mobile application (React Native)
- Advanced analytics and reporting

See [future.md](future.md) and [ideas.md](ideas.md) for detailed plans.

---

## Project Type

- Personal project
- Educational / experimental
- Backend-focused
- Architecture-driven

This project should be viewed as:
- a demonstration of architectural thinking
- a learning baseline
- a foundation for future, more compact systems

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

---

## Final Note

This project is not about speed and not about building a perfect CRUD system.

It is about understanding:
- where architecture helps
- where it starts to slow development
- and what architectural purity realistically costs in solo development
