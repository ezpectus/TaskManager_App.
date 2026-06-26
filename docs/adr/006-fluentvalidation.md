# ADR-006: Use FluentValidation for input validation

**Status:** Accepted  
**Date:** 2025-06-01

## Context

The API receives user input via DTOs. All input must be validated before processing. Candidates considered:

- **FluentValidation** — fluent API for building validation rules, separate from DTOs
- **Data Annotations** — `[Required]`, `[MaxLength]` attributes on DTO properties
- **Manual validation** — `if (string.IsNullOrEmpty(dto.Title)) return BadRequest(...)`
- **MiniValidation** — lightweight, source-generator based

The project has ~15 DTOs with complex validation rules (e.g., email format, password strength, deadline must be in the future, enum value checking).

## Decision

Use **FluentValidation 12.1.0** with separate validator classes per DTO. Validators are registered in DI and automatically invoked by the ASP.NET Core model binding pipeline via `AddFluentValidationAutoValidation()`.

**Note on v12 API change:** FluentValidation 12.x deprecated the `AddFluentValidation()` extension on `IMvcBuilder`. The new approach uses `AddFluentValidationAutoValidation()` and `AddFluentValidationClientsideAdapters()` on `IServiceCollection`, with `AddValidatorsFromAssemblyContaining<T>()` for registration.

## Consequences

**Positive:**
- Validation rules are expressive and readable: `RuleFor(x => x.Title).NotEmpty().MaximumLength(200)`
- Validators are testable in isolation — no HTTP context needed
- Complex rules supported: conditional validation, cross-field validation, async validation
- Separation of concerns — DTOs remain pure data structures, validation logic lives in separate classes
- Automatic validation — no need to call `ModelState.IsValid` in every controller action
- Detailed error messages — FluentValidation returns structured error responses with property names and messages

**Negative:**
- Additional class per DTO (validator) — more files to maintain
- FluentValidation 12.x has breaking API changes from v11 — required migration from `AddFluentValidation()` to `AddFluentValidationAutoValidation()`
- `ImplicitlyValidateChildProperties` is deprecated — child validation should use `SetValidator()` instead

**Neutral:**
- FluentValidation is the most popular validation library in .NET — using it makes the codebase familiar to other .NET developers
