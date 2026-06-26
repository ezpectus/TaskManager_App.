# Architecture Decision Records (ADR)

This directory contains Architecture Decision Records for the Task Manager App.

ADRs document important architectural decisions, their context, and consequences.

## Index

| ADR | Title | Status |
|-----|-------|--------|
| [001](001-postgresql.md) | Use PostgreSQL as the primary database | Accepted |
| [002](002-clean-architecture.md) | Adopt Clean Architecture with DDD concepts | Accepted |
| [003](003-bcrypt-password-hashing.md) | Use BCrypt for password hashing | Accepted |
| [004](004-automapper.md) | Use AutoMapper for entity-to-DTO mapping | Accepted |
| [005](005-serilog-structured-logging.md) | Use Serilog for structured logging | Accepted |
| [006](006-fluentvalidation.md) | Use FluentValidation for input validation | Accepted |

## ADR Format

Each ADR follows this structure:

- **Title** — short noun phrase
- **Status** — Proposed, Accepted, Rejected, Deprecated, Superseded
- **Context** — what is the issue being addressed?
- **Decision** — what is the change being made?
- **Consequences** — what becomes easier or harder after this decision?
