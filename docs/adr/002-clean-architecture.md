# ADR-002: Adopt Clean Architecture with DDD concepts

**Status:** Accepted  
**Date:** 2025-06-01

## Context

The project needed an architectural style that:
- Separates business logic from infrastructure concerns
- Makes the domain layer testable without database or HTTP dependencies
- Enforces dependency direction (inner layers do not depend on outer layers)
- Supports long-term maintainability and understanding of code structure

Candidates considered:
- **Clean Architecture** (Jason Taylor / Robert C. Martin) — layered, dependency-inverted, domain-centric
- **Vertical Slice Architecture** (Jimmy Bogard) — feature-based folders, no layers
- **Traditional N-Tier** — simple Data/Logic/UI layers, no dependency inversion
- **Microservices** — overkill for a solo educational project

## Decision

Adopt **Clean Architecture** with 4 primary layers (Domain, Application, Infrastructure, API) plus a Test project. Incorporate selected DDD concepts:
- Entity factory methods (`TaskItem.Create()`)
- Private setters with state transition methods (`ChangeStatus()`, `MarkAsCompleted()`)
- Soft delete with query filters
- Optimistic concurrency via RowVersion
- Unit of Work pattern for transactional consistency

## Consequences

**Positive:**
- Domain layer has zero external dependencies — fully testable in isolation
- Clear separation of concerns — each layer has a single responsibility
- Dependency direction enforced at compile time (project references)
- DDD entity encapsulation prevents invalid state transitions (e.g., reopening completed tasks)
- Unit of Work ensures transactional consistency across multiple repository operations

**Negative:**
- More boilerplate compared to Vertical Slice (interfaces, DTOs, mappers per entity)
- Over-engineered for a simple CRUD app — acknowledged as a learning trade-off
- More files and indirection — a simple feature change touches multiple layers
- AutoMapper profile configuration adds complexity

**Neutral:**
- The architecture is intentionally over-engineered for educational purposes — this is a deliberate decision, not an accident
