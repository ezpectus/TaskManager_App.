# Extension Ideas & Detailed Specifications

This document contains **detailed functional specifications** for potential extensions and features. It serves as the companion to [future.md](future.md), which defines the **timeline and priority** for each feature.

Each idea below includes a concept description, detailed functionality, and data model extensions where applicable.

---

## 1. Project & Board System

### Concept
Extend the current flat task list into a multi-project, multi-board system similar to Trello or Jira.

### Detailed Functionality

- **Projects**: Top-level container for related tasks
  - Name, description, color, icon
  - Owner and team members
  - Default priority and status pipeline
  - Archived state

- **Boards**: Visual views within a project
  - Kanban board (current, extended with configurable columns)
  - List view (table with sortable columns)
  - Calendar view (tasks on deadline dates)
  - Timeline view (Gantt chart with dependencies)
  - Custom views saved per user

- **Swimlanes**: Horizontal groupings within Kanban
  - Group by assignee, priority, tag, or custom field
  - Collapsible swimlanes
  - Per-swimlane WIP limits

### Data Model Extensions
```
Project: Id, Name, Description, Color, OwnerId, CreatedAt, IsArchived
Board: Id, ProjectId, Name, Type (Kanban|List|Calendar|Timeline), Config (JSON)
BoardColumn: Id, BoardId, Name, Order, StatusMapping, WipLimit
```

---

## 2. Custom Fields & Dynamic Schema

### Concept
Allow users to define custom fields per project (text, number, date, select, multiselect, user, URL).

### Detailed Functionality
- Admin defines custom fields per project
- Fields appear in task create/edit forms
- Fields are filterable and sortable in list views
- Fields can be required or optional
- Default values and validation rules
- Field types: text, number, date, select, multiselect, user reference, URL, checkbox

### Implementation Approach
- **EAV (Entity-Attribute-Value)** pattern for maximum flexibility
- Or JSON column on TaskItem for simpler projects
- Custom field definitions stored in separate table
- UI renders fields dynamically based on project config

### Data Model
```
CustomField: Id, ProjectId, Name, Type, IsRequired, DefaultValue, Options (JSON), Order
CustomFieldValue: Id, TaskId, FieldId, Value (string, parsed by type)
```

---

## 3. Automation Rules Engine

### Concept
Allow users to define "when X happens, do Y" rules to automate repetitive workflows.

### Detailed Functionality
- **Triggers**: Task created, status changed, priority changed, assigned, commented, deadline approaching
- **Conditions**: Field matches value, tag contains, priority is, assignee is
- **Actions**: Set field, add tag, assign user, send notification, create subtask, move to status, set deadline
- Rule priority and conflict resolution
- Rule execution log
- Pre-built templates (e.g., "Auto-assign high-priority tasks to team lead")

### Example Rules
```
Rule 1: WHEN task.priority = High AND task.status = Todo
        THEN assign to @teamlead AND send notification

Rule 2: WHEN task.status = Done
        THEN mark all subtasks as completed AND log activity

Rule 3: WHEN task.deadline < 2 days AND task.status != Done
        THEN set priority to High AND send reminder
```

### Data Model
```
AutomationRule: Id, ProjectId, Name, TriggerType, Conditions (JSON), Actions (JSON), IsEnabled
AutomationLog: Id, RuleId, TaskId, ExecutedAt, Result
```

---

## 4. Time Tracking & Timesheets

### Concept
Built-in time tracking for tasks with reporting and export capabilities.

### Detailed Functionality
- **Timer**: Start/stop/pause timer on any task
  - Running timer indicator in navbar
  - Auto-stop on task completion
  - Multiple time entries per task
  - Manual time entry with date and duration

- **Timesheet View**:
  - Weekly grid: rows = tasks, columns = days
  - Enter hours per task per day
  - Submit timesheet for approval
  - Export to CSV, Excel, PDF

- **Reports**:
  - Time by user, project, tag, priority
  - Billable vs non-billable
  - Date range filters
  - Charts: bar, pie, heatmap

### Data Model
```
TimeEntry: Id, TaskId, UserId, StartTime, EndTime, Duration, Description, IsBillable
Timesheet: Id, UserId, WeekStart, Status (Draft|Submitted|Approved), Entries
```

---

## 5. Advanced Search & Saved Filters

### Concept
Powerful search with complex queries and saveable filter presets.

### Detailed Functionality
- **Search bar**: Full-text search across title, description, comments
- **Filter builder**: AND/OR conditions on any field
  - Status, priority, assignee, tag, deadline, custom fields
  - Date ranges (created, updated, deadline)
  - Relative dates (today, this week, overdue)
- **Saved filters**: Name and pin frequently used filters
  - Share filters with team
  - Default filter per board/project
- **Quick filters**: One-click buttons for common queries
  - My tasks, Overdue, High priority, Recently updated

### UI Components
- Filter chips showing active filters
- Remove individual filters
- Combine multiple filters with AND/OR logic
- Filter persistence via URL query params

---

## 6. Task Templates & Recurring Tasks

### Concept
Create reusable task templates and set up recurring tasks on schedules.

### Detailed Functionality
- **Task Templates**:
  - Save a task as template (with subtasks, tags, description)
  - Create task from template
  - Template library per project
  - Pre-filled fields with overrides

- **Recurring Tasks**:
  - Recurrence patterns: daily, weekly, monthly, custom (cron-like)
  - Auto-generate next instance when current is completed
  - Or generate on schedule regardless of completion
  - Recurrence end date or infinite
  - Preview upcoming instances

### Data Model
```
TaskTemplate: Id, ProjectId, Name, Title, Description, Priority, Tags (JSON), Subtasks (JSON)
RecurrenceRule: Id, TaskId, Pattern, Interval, DaysOfWeek, DayOfMonth, EndDate, NextGenerationAt
```

---

## 7. File Attachments & Rich Content

### Concept
Enhance tasks with file uploads, images, and rich text descriptions.

### Detailed Functionality
- **File Uploads**:
  - Drag-and-drop file upload on tasks
  - Image preview thumbnails
  - File size limits and type restrictions
  - S3-compatible storage backend
  - Virus scanning integration

- **Rich Text Descriptions**:
  - Markdown editor with live preview
  - Code blocks with syntax highlighting
  - Image embedding
  - Table support
  - Link previews

- **Comment Enhancements**:
  - Markdown in comments
  - Image attachments in comments
  - @mentions with autocomplete
  - Emoji reactions on comments

---

## 8. Team Collaboration Features

### Concept
Multi-user collaboration with real-time updates and social features.

### Detailed Functionality
- **Real-Time Collaboration** (SignalR):
  - Live cursor positions on Kanban board
  - Real-time task card updates
  - User presence (online/offline/away)
  - Live comment stream
  - Conflict resolution for concurrent edits

- **Mentions & Notifications**:
  - @user mentions in comments and descriptions
  - @team mentions
  - Notification preferences per user
  - Email digest (daily/weekly summary)
  - Push notifications (PWA)

- **Shared Views**:
  - Share board link with view-only access
  - Public read-only boards
  - Embeddable board widget

---

## 9. Analytics & Reporting

### Concept
Data-driven insights into team productivity and project health.

### Detailed Functionality
- **Dashboard Widgets**:
  - Task completion rate (weekly/monthly)
  - Average time to completion
  - Overdue task count
  - Workload distribution per user
  - Tag distribution pie chart
  - Burndown chart for sprint tracking

- **Custom Reports**:
  - Report builder with drag-and-drop widgets
  - Date range and filter selection
  - Export to PDF, CSV, JSON
  - Scheduled email reports

- **Velocity Metrics**:
  - Story points per sprint (if using agile)
  - Cumulative flow diagram
  - Cycle time and lead time
  - Throughput trend

---

## 10. Integration & API Extensions

### Concept
Open the platform for integrations with external tools.

### Detailed Functionality
- **Webhooks**:
  - Subscribe to task events (created, updated, deleted, commented)
  - Configurable per project
  - Retry with exponential backoff
  - Webhook delivery logs

- **REST API v2**:
  - OAuth 2.0 for third-party apps
  - Rate limiting
  - API key authentication
  - GraphQL endpoint for flexible queries
  - SDK generation (TypeScript, Python, C#)

- **Integrations**:
  - Slack: task notifications in channels
  - GitHub: link commits and PRs to tasks
  - Google Calendar: sync deadlines
  - Zapier/Make: connect to 5000+ apps

---

## 11. Dark Mode & UI Polish — ✅ Mostly Implemented

### Concept
Polish the UI with dark mode, themes, and UX improvements.

### Implemented
- **Dark Mode**: toggle with persistence ✅
- **Keyboard Shortcuts**: `?` overlay ✅
- **Skeleton Loaders**: loading placeholders ✅
- **Toasts**: success/error notifications ✅
- **Empty States**: messages with calls to action ✅
- **Priority Icons**: arrow up/down/minus/alert (incl. Critical) ✅
- **4-status Kanban**: Todo, In Progress, Done, Cancelled ✅
- **Comment author attribution**: username on comments ✅
- **Auto-open edit mode**: via `?edit=true` URL param ✅

### Still TODO
- **Themes**: Custom accent colors, compact/comfortable density
- **Keyboard Shortcuts**: `N` — new task, `/` — focus search, `B` — toggle board/list view, `G` then `D` — go to dashboard, `G` then `K` — go to kanban
- **Drag & Drop**: Reorder subtasks within a task
- **Calendar View**: Month/week view with tasks on deadline dates (Google Calendar style)
- **Timeline View**: Gantt chart with task dependencies

---

## 12. Security & Compliance

### Concept
Enterprise-grade security features for production use.

### Detailed Functionality
- **Authentication**:
  - OAuth 2.0 / OpenID Connect (Google, GitHub, Microsoft)
  - Two-factor authentication (TOTP, SMS)
  - Session management and device tracking
  - Password policy enforcement

- **Authorization**:
  - Role-based access control (Admin, Manager, Member, Viewer)
  - Project-level permissions
  - API token scoping

- **Audit & Compliance**:
  - Comprehensive audit log (who did what, when)
  - Data export (GDPR compliance)
  - Data retention policies
  - Soft delete with configurable retention period

---

## 13. Calendar View (Google Calendar / Notion style)

### Concept
Display tasks on a calendar grid, grouped by deadline date — inspired by Google Calendar's month/week views and Notion's calendar database view.

### Detailed Functionality
- **Month View**: Grid of 4–6 weeks, tasks rendered as colored chips on their deadline date
- **Week View**: 7-day columns with tasks positioned by deadline time
- **Day View**: Hourly slots with timed tasks
- **Color coding**: By priority (Low=gray, Medium=orange, High=red, Critical=purple) or by status
- **Drag & drop**: Drag task chip to different date to change deadline
- **Quick create**: Click empty date slot to create task with that deadline
- **Mini calendar**: Sidebar month picker for navigation
- **Today indicator**: Highlight current date cell
- **Overflow handling**: "+N more" on cells with too many tasks

### Data Model
No new tables — uses existing `TaskItem.Deadline` field. Frontend-only feature.

---

## 14. Task Dependencies & Blocked Status (Jira / Asana style)

### Concept
Allow tasks to depend on other tasks — a task cannot start until its blockers are done.

### Detailed Functionality
- **Blocked-by relationship**: Task A is blocked by Task B → Task A shows "Blocked" badge
- **Blocking relationship**: Task B blocks Task A → Task B shows "Blocking N tasks" badge
- **Circular dependency detection**: Prevent A→B→C→A cycles
- **Auto-status**: When all blockers complete, task auto-changes from "Blocked" to "Todo"
- **Dependency graph**: Visual node-link diagram showing task relationships
- **Cascade complete**: Option to auto-complete dependent tasks when blocker is done

### Data Model
```
TaskDependency: Id, TaskId (dependent), BlocksOnTaskId (blocker), CreatedAt
```

---

## 15. Recurring Tasks & Templates (Todoist / TickTick style)

### Concept
Create tasks that automatically regenerate on a schedule, and reusable task templates.

### Detailed Functionality
- **Recurrence patterns**: Daily, weekly (specific days), monthly, custom (every N days/weeks/months)
- **Auto-generation**: Next instance created when current is completed or on schedule date
- **End conditions**: After N occurrences, on specific date, or never
- **Template library**: Save task as template with subtasks, tags, description, priority
- **Create from template**: One-click task creation with pre-filled fields
- **Preview upcoming**: Show next 5 scheduled instances

### Data Model
```
TaskTemplate: Id, Name, Title, Description, Priority, Subtasks (JSON), Tags (JSON)
RecurrenceRule: Id, TaskId, Pattern, Interval, DaysOfWeek, DayOfMonth, EndDate, NextGenerationAt, MaxOccurrences
```

---

## 16. Natural Language Task Creation (Todoist / TickTick style)

### Concept
Create tasks by typing natural language — the system parses intent, deadline, and priority automatically.

### Detailed Functionality
- **Smart parsing**: Type "Fix login bug tomorrow high priority" → creates task with:
  - Title: "Fix login bug"
  - Deadline: tomorrow
  - Priority: High
- **Date keywords**: today, tomorrow, next week, Monday, Jan 15, in 3 days
- **Priority keywords**: low, medium, high, critical, !1, !2, !3, !4
- **Quick add bar**: Global keyboard shortcut (e.g., `Ctrl+Space`) opens quick-add input
- **Preview**: Show parsed fields before creating task
- **Undo**: Quick undo if parsing was wrong

### Implementation
- Frontend-only parsing with regex + date library (no AI needed for basic version)
- Optional: LLM integration for complex natural language input
