# Architecture Decisions

This document describes the technical architecture of the Task Manager App.
It focuses only on implementation decisions and trade-offs.

---

## Layers

The project follows a layered architecture inspired by Clean Architecture.

- API — controllers and HTTP contracts
- Application — services and business logic
- Domain — core domain entities
- Persistence — EF Core configuration and database access
- Infrastructure — infrastructure-level dependencies
- Tests — reserved layer for testing

Inner layers are isolated from outer layers.

---

## DTO ≠ Request

DTOs and request models are intentionally separated.

Request models:
- represent client input
- define what data can be sent from outside

DTOs:
- represent internal or output data
- may include computed or related information

This separation prevents API coupling and accidental contract changes.

---

## EF Core Usage

EF Core is used to map an existing PostgreSQL schema.

The database is created manually.
EF Core is not responsible for schema generation.

Responsibilities of EF Core in this project:
- entity mapping
- relationships
- constraints
- data access

---

## Why Fluent API

All entity configuration is done using Fluent API.

Reasons:
- centralized configuration
- no EF-specific logic inside domain entities
- full control over indexes, keys, and cascade rules

Trade-off:
- more boilerplate
- slower development

Benefit:
- predictable and transparent schema behavior

---

## Why No Unit of Work

A custom Unit of Work abstraction was removed.

Reasons:
- EF Core already provides transaction management
- extra abstraction increased complexity
- debugging became harder

Repositories interact directly with DbContext.

---

## Where Architecture Became Heavy

Examples:
- one repository per entity
- strict separation everywhere
- manual mappings even for simple queries

This was intentional and done for learning purposes.

---

## Conclusions

- Architecture is a tool, not a goal
- Over-abstraction is expensive in solo development
- Understanding trade-offs is more valuable than blind purity

