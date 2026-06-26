# ADR-001: Use PostgreSQL as the primary database

**Status:** Accepted  
**Date:** 2025-06-01

## Context

The project needed a relational database for storing users, tasks, subtasks, comments, and related entities. The main candidates were:

- **PostgreSQL** — open-source, cross-platform, ACID-compliant
- **SQL Server** — Microsoft ecosystem, Windows-centric, licensing costs for production
- **SQLite** — lightweight, file-based, no server needed
- **MySQL** — open-source, widely used, but less feature-rich than PostgreSQL

The project targets .NET 9 on multiple platforms (Windows, Linux, Docker). The database must support:
- JSON columns (for future flexibility)
- Full-text search (for task filtering)
- Row versioning (for optimistic concurrency)
- Docker-friendly deployment

## Decision

Use **PostgreSQL 16** as the primary database, accessed via **EF Core 9** with the **Npgsql** provider.

## Consequences

**Positive:**
- No licensing costs — fully open-source
- Excellent cross-platform support (Linux, Windows, macOS, Docker)
- First-class EF Core provider (Npgsql) maintained by the .NET community
- Native UUID support for primary keys (no need for sequential GUIDs)
- JSONB column support for future schema flexibility
- `xmin` system column for optimistic concurrency (used via RowVersion)
- Docker image available (`postgres:16-alpine`) — seamless containerized deployment

**Negative:**
- Requires a running PostgreSQL instance (unlike SQLite)
- Npgsql has some EF Core feature limitations compared to SQL Server provider
- No GUI tools as polished as SQL Server Management Studio (pgAdmin is adequate but not equivalent)

**Neutral:**
- Database schema is managed via Fluent API configuration, not migrations (by design — see overview.md)
