# ADR-003: Use BCrypt for password hashing

**Status:** Accepted  
**Date:** 2025-06-01

## Context

The application stores user credentials and must hash passwords securely. Candidates considered:

- **BCrypt** — adaptive hash function, industry standard, built-in salt generation
- **Argon2** — winner of the Password Hashing Competition (2015), memory-hard
- **PBKDF2** — NIST-standard, key derivation function, widely available
- **SCrypt** — memory-hard, less widely adopted than BCrypt

The project uses .NET 9, which does not include a built-in password hasher (ASP.NET Core Identity has one, but the project does not use Identity).

## Decision

Use **BCrypt.Net-Next** (v4.0.3) for password hashing. This is an actively maintained fork of the original BCrypt.Net library.

Implementation:
- `BCrypt.Net.BCrypt.HashPassword(password)` during user registration
- `BCrypt.Net.BCrypt.Verify(password, hash)` during login
- Default work factor of 12 (can be increased over time)

## Consequences

**Positive:**
- Industry-standard algorithm with proven security track record
- Adaptive cost factor — can be increased as hardware improves
- Built-in salt generation — no need to manage salts manually
- Simple API: `HashPassword()` and `Verify()` — two methods, that's it
- BCrypt.Net-Next is actively maintained and supports .NET 9

**Negative:**
- BCrypt is CPU-intensive by design (work factor 12 = ~250ms per hash) — login throughput is limited
- Not as modern as Argon2 (which is memory-hard and resistant to GPU attacks)
- BCrypt.Net-Next is a community fork, not a Microsoft first-party package

**Neutral:**
- For an educational project, BCrypt provides the best balance of security, simplicity, and industry recognition
- Argon2 would be the choice for a production system with higher security requirements
