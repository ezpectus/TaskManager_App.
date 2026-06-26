# ADR-005: Use Serilog for structured logging

**Status:** Accepted  
**Date:** 2025-06-01

## Context

The application needs logging for debugging, monitoring, and audit purposes. Candidates considered:

- **Serilog** — structured logging library with rich sink ecosystem
- **Built-in ILogger** — Microsoft.Extensions.Logging, default in ASP.NET Core
- **NLog** — traditional logging framework, XML configuration
- **ZLogger** — zero-allocation logging, high performance

The project needs:
- Structured logging (key-value pairs, not just strings)
- Multiple sinks (Console for dev, File for production, potentially Elasticsearch)
- Easy integration with ASP.NET Core middleware pipeline
- Request logging with HTTP method, path, status code, and duration

## Decision

Use **Serilog 9.0.0** with the following sinks:
- `Serilog.Sinks.Console` — colored console output for development
- `Serilog.Sinks.File` — rolling daily file logs in `logs/` directory

Configuration via `appsettings.json` (`Serilog:MinimumLevel`, `Serilog:WriteTo`) with code-based bootstrap for early startup logging.

## Consequences

**Positive:**
- Structured logging enables powerful querying: `Log.Information("Task {TaskId} created by {UserId}", taskId, userId)` produces JSON with named properties
- Rich sink ecosystem — can add Elasticsearch, Seq, Datadog, Application Insights without code changes
- `Serilog.AspNetCore` integrates seamlessly with ASP.NET Core — replaces built-in `ILogger`
- Request logging middleware automatically logs every HTTP request with method, path, status, duration
- Configuration via `appsettings.json` — no recompilation needed to change log levels

**Negative:**
- Additional NuGet dependencies (Serilog, Serilog.Sinks.Console, Serilog.Sinks.File, Serilog.AspNetCore)
- Slightly more complex setup than built-in ILogger
- File sink writes to disk — need log rotation and cleanup strategy for production

**Neutral:**
- Serilog is the most popular structured logging library in .NET — widely recognized by senior developers
