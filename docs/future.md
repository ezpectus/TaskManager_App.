# Roadmap

This document outlines the planned evolution of the Task Manager App beyond its current full-stack state. It serves as a **timeline and priority guide** — for detailed feature specifications, see [ideas.md](ideas.md).

---

## Current State (v1.7)

The application is a fully functional full-stack task manager with:
- **Backend**: ASP.NET Core 9, Clean Architecture, JWT auth + refresh tokens, EF Core + PostgreSQL
- **Frontend**: React 19, TypeScript, TailwindCSS, Kanban board with drag & drop, dashboard with filters, analytics page, markdown rendering, CSV export, profile editing with password change
- **Testing**: 60 unit tests (xUnit + Moq + AutoMapper)
- **DevOps**: Docker, GitHub Actions CI/CD, docker-compose, nginx, start.bat (Windows) + start.sh (Linux)
- **Features**: Rate limiting, response caching, activity logging, DB seeding, task assignment, soft delete, optimistic concurrency
- **Documentation**: ADRs, API reference, changelog, contributing guide, refactoring log

---

## Phase 1: Stability & DevOps — Completed

- [x] Docker containerization (multi-stage builds for backend + frontend)
- [x] CI/CD pipeline (GitHub Actions: build + test on push)
- [x] Database seeding on startup (admin/demo users, roles, sample tasks)
- [x] Cross-platform startup scripts (start.bat, start.sh)

---

## Phase 2: Real-Time & Notifications (Medium-term)

**Timeline:** 3-4 weeks

- SignalR integration: real-time task updates, live Kanban sync, user presence
- Email notifications: SMTP for assignment, deadline reminders, completion summaries
- In-App notifications: bell icon, read/unread state, per-user preferences

> **Detailed spec:** See [ideas.md — Team Collaboration Features](ideas.md#8-team-collaboration-features)

---

## Phase 3: Advanced Features (Medium-term)

**Timeline:** 4-6 weeks

- Task dependencies: blocked-by/blocking, graph visualization, circular detection
- Recurring tasks: daily/weekly/monthly patterns, auto-generation, templates
- Time tracking: start/stop timer, manual entry, reports, billable hours
- Custom workflows: configurable pipelines, custom fields, automation rules

> **Detailed specs:**
> - Recurring tasks & templates: [ideas.md — Task Templates & Recurring Tasks](ideas.md#6-task-templates--recurring-tasks)
> - Time tracking: [ideas.md — Time Tracking & Timesheets](ideas.md#4-time-tracking--timesheets)
> - Custom fields: [ideas.md — Custom Fields & Dynamic Schema](ideas.md#2-custom-fields--dynamic-schema)
> - Automation rules: [ideas.md — Automation Rules Engine](ideas.md#3-automation-rules-engine)

---

## Phase 4: Collaboration (Long-term)

**Timeline:** 3-4 weeks

- Team management: organizations, RBAC, project-level permissions
- Rich comments: markdown, @mentions, threading, file attachments
- Activity feed: global stream, filterable, audit trail, CSV/JSON export

> **Detailed spec:** See [ideas.md — Team Collaboration Features](ideas.md#8-team-collaboration-features)

---

## Phase 5: Mobile & PWA (Long-term)

**Timeline:** 4-6 weeks

- Progressive Web App: service worker, offline support, push notifications, installable
- React Native mobile app: iOS + Android, shared API client, offline-first sync, biometric auth

---

## Phase 6: Analytics & AI (Future)

**Timeline:** 4-8 weeks

- Analytics dashboard: completion rates, velocity charts, time distribution, custom reports
- AI-powered features: smart prioritization, auto-tagging, deadline estimation, natural language task creation

> **Detailed spec:** See [ideas.md — Analytics & Reporting](ideas.md#9-analytics--reporting)

---

## Technology Roadmap

| Phase | Key Technologies | Timeline | Status |
|-------|-----------------|----------|--------|
| 1 | Docker, GitHub Actions, EF Migrations | — | Done |
| 2 | SignalR, SMTP, Notification system | 3-4 weeks | Planned |
| 3 | Dependencies, Recurring tasks, Time tracking, Automation | 4-6 weeks | Planned |
| 4 | Teams, RBAC, Rich comments, Activity feed | 3-4 weeks | Planned |
| 5 | PWA, React Native | 4-6 weeks | Planned |
| 6 | Analytics, AI integration | 4-8 weeks | Planned |

---

## Migration Considerations

When the application scales beyond a single instance, the following infrastructure changes should be considered:

- **Database**: PostgreSQL read replicas for analytics workloads
- **Caching**: Redis for session storage and frequently accessed data
- **Message Queue**: RabbitMQ or Kafka for async notifications and webhooks
- **Search**: Elasticsearch or PostgreSQL full-text search for advanced filtering
- **File Storage**: S3-compatible storage for attachments
- **Monitoring**: OpenTelemetry + Grafana or Application Insights
