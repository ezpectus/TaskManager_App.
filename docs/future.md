# Future Plans

This document outlines the planned evolution of the Task Manager App beyond its current full-stack state.

---

## Current State (Completed)

The application is a fully functional full-stack task manager with:
- Backend: ASP.NET Core 9, Clean Architecture, JWT auth, EF Core + PostgreSQL
- Frontend: React 19, TypeScript, TailwindCSS, Kanban board, dashboard
- Testing: 30+ unit tests (xUnit + Moq)
- DevOps: `.editorconfig`, structured logging, health checks

---

## Phase 1: Stability & DevOps (Short-term)

### Docker Containerization
- `Dockerfile` for backend (multi-stage build)
- `Dockerfile` for frontend (Vite build + nginx serve)
- `docker-compose.yml` with PostgreSQL, API, and frontend services
- Volume mapping for database persistence
- Environment-based configuration

### CI/CD Pipeline
- GitHub Actions workflow for:
  - Build and test on push
  - Lint and format check
  - Docker image build and push on merge to main
- Branch protection rules
- Automated PR checks

### Database Migrations
- EF Core migration scripts
- Migration runner in `Program.cs`
- Seed data for development

---

## Phase 2: Real-Time & Notifications (Medium-term)

### SignalR Integration
- Real-time task updates (create, update, delete)
- Live Kanban board synchronization
- User presence indicators
- Notification hub for mentions and assignments

### Email Notifications
- SMTP integration for:
  - Task assignment notifications
  - Deadline reminders
  - Task completion summaries
- Template-based email rendering
- Unsubscribe and preference management

### In-App Notifications
- Notification bell in navbar
- Read/unread state
- Notification preferences per user

---

## Phase 3: Advanced Features (Medium-term)

### Task Dependencies
- Blocked-by / blocking relationships
- Dependency graph visualization
- Circular dependency detection
- Auto-status updates when dependencies complete

### Recurring Tasks
- Daily, weekly, monthly recurrence patterns
- Auto-generation of task instances
- Template-based task creation
- Recurrence preview calendar

### Time Tracking
- Start/stop timer on tasks
- Manual time entry
- Time reports per user, project, tag
- Billable vs non-billable hours

### Custom Workflows
- Configurable status pipelines per project
- Custom fields (text, number, date, select)
- Automation rules (trigger → action)
- Webhook integration for external systems

---

## Phase 4: Collaboration (Long-term)

### Team Management
- Organizations and teams
- Role-based access control (RBAC)
- Project-level permissions
- Team dashboards and analytics

### Comments & Mentions
- Rich text comments with markdown
- @mentions with notifications
- Comment threading and replies
- File attachments in comments

### Activity Feed
- Global activity stream
- Filterable by user, action type, entity
- Audit trail for compliance
- Export to CSV/JSON

---

## Phase 5: Mobile & PWA (Long-term)

### Progressive Web App
- Service worker for offline support
- Installable on mobile devices
- Push notifications via web push API
- Responsive design optimization

### React Native Mobile App
- iOS and Android apps
- Shared API client with web
- Offline-first with sync
- Biometric authentication

---

## Phase 6: Analytics & AI (Future)

### Analytics Dashboard
- Task completion rates
- Velocity charts (burndown, cumulative flow)
- Time distribution by tag, priority, user
- Custom report builder

### AI-Powered Features
- Smart task prioritization
- Auto-tagging based on content
- Deadline estimation based on historical data
- Natural language task creation ("Create a high-priority task to fix the login bug by Friday")

---

## Technology Roadmap

| Phase | Technologies | Timeline |
|-------|-------------|----------|
| 1 | Docker, GitHub Actions, EF Migrations | 2-3 weeks |
| 2 | SignalR, SMTP, Notification system | 3-4 weeks |
| 3 | Dependencies, Recurring tasks, Time tracking | 4-6 weeks |
| 4 | Teams, RBAC, Rich comments | 3-4 weeks |
| 5 | PWA, React Native | 4-6 weeks |
| 6 | Analytics, AI integration | 4-8 weeks |

---

## Migration Considerations

- **Database**: PostgreSQL → consider read replicas for analytics workloads
- **Caching**: Redis for session storage and frequently accessed data
- **Message Queue**: RabbitMQ or Kafka for async notifications and webhooks
- **Search**: Elasticsearch or PostgreSQL full-text search for advanced filtering
- **File Storage**: S3-compatible storage for attachments
- **Monitoring**: Application Insights or OpenTelemetry + Grafana
