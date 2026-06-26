# ADR-004: Use AutoMapper for entity-to-DTO mapping

**Status:** Accepted  
**Date:** 2025-06-01  
**Updated:** 2026-06-22 — upgraded to v15.1.3 (security fix for GHSA-rvv3-g6hj-g44x)

## Context

The application has separate DTOs for API contracts and entities for domain logic. Manual mapping between them requires significant boilerplate. Candidates considered:

- **AutoMapper** — convention-based object mapper, widely used in .NET
- **Manual mapping** — explicit `new Dto { Prop = entity.Prop }` for each field
- **Mapster** — faster alternative to AutoMapper, similar API
- **Expression-based mapping** — compile-time generated mappers (e.g., Mapperly)

The project has ~10 entities with multiple DTOs each. Manual mapping would require ~30 mapping methods with repetitive property assignments.

## Decision

Use **AutoMapper 15.1.3** with a single `MappingProfile` that defines all entity-to-DTO and DTO-to-entity mappings.

**Note on v15 breaking change:** AutoMapper 15.x requires an `ILoggerFactory` parameter in the `MapperConfiguration` constructor. In tests, `NullLoggerFactory.Instance` is used. In production, the DI container provides the logger factory automatically.

## Consequences

**Positive:**
- Eliminates ~300 lines of manual mapping boilerplate
- Convention-based: properties with matching names and types are mapped automatically
- Centralized mapping configuration in a single `MappingProfile` class
- Supports complex scenarios (collections, nested objects, custom resolvers)
- Well-documented, large community, extensive Stack Overflow coverage

**Negative:**
- Reflection-based — has a performance cost (mitigated by compiled mapping plans after first use)
- AutoMapper 15.x has a known vulnerability (GHSA-rvv3-g6hj-g44x) — fixed in 15.1.3
- Configuration errors surface at runtime, not compile time (unlike source generators like Mapperly)
- The `ILoggerFactory` requirement in v15+ adds a dependency in test setup

**Neutral:**
- AutoMapper is the de facto standard in .NET Clean Architecture projects — using it makes the codebase familiar to other .NET developers
