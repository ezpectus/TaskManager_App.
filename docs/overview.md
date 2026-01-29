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

At its current stage, the project includes a fully working backend foundation:

- ASP.NET Core Web API
- Layered architecture with clear responsibility boundaries
- Domain entities modeled around real database constraints
- Manually designed PostgreSQL schema
- Entity Framework Core used for mapping an existing database schema
- Repository and service layers
- Relationships between entities with explicit configuration
- Business logic separated from API concerns
- RESTful API endpoints

The backend logic is functional and internally consistent.

---

### What Is Not Implemented

The following components are intentionally not implemented yet:

- Frontend application
- Authentication and authorization
- Notifications
- Swagger / OpenAPI documentation
- Messaging and asynchronous communication
- Production-level polishing and optimization

The absence of these features is a conscious decision driven by time constraints, not by a lack of understanding or experience.

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

A custom Unit of Work abstraction was initially considered but later removed.

Reasons:
- EF Core already manages transactions and change tracking
- additional abstraction increased complexity without clear benefits
- transparency and debuggability were prioritized over architectural formality

Repositories interact directly with the DbContext.

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

If development continues in the future, potential directions include:
- frontend application (Blazor or React)
- JWT-based authentication and authorization
- notifications system
- messaging via RabbitMQ or Kafka
- Swagger / OpenAPI integration
- extraction of additional services

These are possible directions, not commitments.

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

- ASP.NET Core Web API
- C#
- PostgreSQL
- Entity Framework Core
- Fluent API
- Clean Architecture (simplified)

---

## Final Note

This project is not about speed and not about building a perfect CRUD system.

It is about understanding:
- where architecture helps
- where it starts to slow development
- and what architectural purity realistically costs in solo development
