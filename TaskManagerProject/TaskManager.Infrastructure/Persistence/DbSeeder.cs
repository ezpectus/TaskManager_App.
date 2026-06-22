using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Persistence.Contexts;

namespace TaskManager.Infrastructure.Persistence;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Roles.AnyAsync()) return;

        var adminRoleId = Guid.NewGuid();
        var userRoleId = Guid.NewGuid();

        await context.Roles.AddRangeAsync(
            new Role { Id = adminRoleId, RoleName = "Admin" },
            new Role { Id = userRoleId, RoleName = "User" }
        );

        var adminId = Guid.NewGuid();
        var demoUserId = Guid.NewGuid();

        var admin = new User
        {
            Id = adminId,
            Username = "admin",
            Email = "admin@taskmanager.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            CreatedAt = DateTime.UtcNow,
        };

        var demoUser = new User
        {
            Id = demoUserId,
            Username = "demo",
            Email = "demo@taskmanager.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Demo123!"),
            CreatedAt = DateTime.UtcNow,
        };

        await context.Users.AddRangeAsync(admin, demoUser);

        await context.UserRoles.AddRangeAsync(
            new UserRole { Id = Guid.NewGuid(), UserId = adminId, RoleId = adminRoleId },
            new UserRole { Id = Guid.NewGuid(), UserId = demoUserId, RoleId = userRoleId }
        );

        var tasks = new[]
        {
            TaskItem.Create("Setup project repository", "Initialize git repo and project structure", TaskPriority.High, DateTime.UtcNow.AddDays(7)),
            TaskItem.Create("Design database schema", "Create ERD and migration scripts", TaskPriority.High, DateTime.UtcNow.AddDays(3)),
            TaskItem.Create("Implement authentication", "JWT-based login and registration", TaskPriority.Medium, DateTime.UtcNow.AddDays(10)),
            TaskItem.Create("Create dashboard UI", "React dashboard with task cards", TaskPriority.Medium, DateTime.UtcNow.AddDays(14)),
            TaskItem.Create("Write unit tests", "Cover TaskService and UserService with tests", TaskPriority.Low, DateTime.UtcNow.AddDays(21)),
            TaskItem.Create("Deploy to staging", "Docker compose setup for staging env", TaskPriority.Low, DateTime.UtcNow.AddDays(30)),
        };

        tasks[0].AssignTo(demoUser);
        tasks[1].AssignTo(demoUser);
        tasks[2].AssignTo(admin);
        tasks[3].AssignTo(demoUser);
        tasks[4].AssignTo(demoUser);
        tasks[5].AssignTo(admin);

        tasks[1].ChangeStatus(Domain.Enums.TaskStatus.InProgress);
        tasks[2].ChangeStatus(Domain.Enums.TaskStatus.Done);

        await context.Tasks.AddRangeAsync(tasks);

        await context.SaveChangesAsync();
    }
}
