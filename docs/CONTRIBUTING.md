# Contributing

This is a personal educational project, but contributions, suggestions, and discussions are welcome.

---

## Development Setup

### Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [PostgreSQL](https://www.postgresql.org/download/) (running on `localhost:5432`)
- [Node.js 20+](https://nodejs.org/) and npm
- [Docker](https://www.docker.com/) (optional, for containerized development)

### Getting Started

1. Clone the repository
2. Configure database connection in User Secrets:
   ```bash
   cd TaskManagerProject/TaskManager.API
   dotnet user-secrets init
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=TaskManagerDb;Username=postgres;Password=YOUR_PASSWORD"
   dotnet user-secrets set "Jwt:Key" "YOUR_SECRET_KEY_AT_LEAST_32_CHARS_LONG"
   ```
3. Build and run:
   ```bash
   dotnet build TaskManagerProject\TaskManagerSolution.sln
   cd TaskManagerProject\TaskManager.API
   dotnet run
   ```
4. Start the frontend:
   ```bash
   cd TaskManagerProject\frontend
   npm install
   npm run dev
   ```

---

## Code Style

### C# / Backend
- **File-scoped namespaces** (`namespace X;`) — not block-scoped
- **Private setters** on entity properties — use domain methods for state changes
- **Fluent API** for EF Core configuration — not Data Annotations
- **FluentValidation** for DTO validation — not Data Annotations
- **AutoMapper profiles** for entity-to-DTO mapping — not manual mapping
- **CancellationToken** passed through all async methods
- **File-scoped namespaces** — all files use `namespace X;` style
- **Naming**: PascalCase for public members, `_camelCase` for private fields

### TypeScript / Frontend
- **Functional components** with hooks — no class components
- **TypeScript strict mode** — no `any` types
- **TailwindCSS** utility classes — no custom CSS files
- **Named exports** — not default exports

### EditorConfig
The project includes `.editorconfig` with style, formatting, and naming rules. Ensure your IDE respects it.

---

## Branch Naming

| Type | Format | Example |
|------|--------|---------|
| Feature | `feature/description` | `feature/task-dependencies` |
| Bug fix | `fix/description` | `fix/kanban-drag-drop` |
| Documentation | `docs/description` | `docs/api-reference` |
| Refactor | `refactor/description` | `refactor/user-service` |

---

## Pull Request Process

1. Create a branch from `main` following the naming convention above
2. Make your changes — keep commits focused and descriptive
3. Ensure the build passes:
   ```bash
   dotnet build TaskManagerProject\TaskManagerSolution.sln
   ```
4. Ensure all tests pass:
   ```bash
   dotnet test TaskManagerProject\TaskManagerSolution.sln
   ```
5. If frontend changes were made, ensure the frontend builds:
   ```bash
   cd TaskManagerProject\frontend
   npm run build
   ```
6. Update documentation if needed (README.md, docs/, CHANGELOG.md)
7. Open a Pull Request with a clear description of what changed and why

---

## Testing

- **Unit tests** are mandatory for new service methods and domain entity behavior
- Use **xUnit + Moq** for backend tests
- Follow the existing test naming convention: `MethodName_Should_Result_When_Condition`
- Test both success and edge case scenarios
- Run tests before submitting a PR:
  ```bash
  dotnet test TaskManagerProject\TaskManagerSolution.sln
  ```

---

## Architecture Guidelines

- **Inner layers do not depend on outer layers** — Domain has zero external dependencies
- **Entities encapsulate invariants** — use factory methods and state transition methods, not public setters
- **Repositories do not call SaveChangesAsync** — use `IUnitOfWork` for transactional consistency
- **DTOs are separate from entities** — never expose domain entities directly to the API
- **All endpoints are versioned** — use `api/v1/...` URL segment

For detailed architecture decisions, see [docs/adr/](docs/adr/).

---

## Questions or Suggestions?

Feel free to open an issue for:
- Bug reports
- Feature suggestions
- Architecture questions
- Documentation improvements
