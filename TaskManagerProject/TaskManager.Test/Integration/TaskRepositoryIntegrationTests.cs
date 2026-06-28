using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Infrastructure.Persistence.Contexts;

using TaskStatus = TaskManager.Domain.Enums.TaskStatus;

namespace TaskManager.Test.Integration;

public class TaskRepositoryIntegrationTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;

    public TaskRepositoryIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync()
    {
        Skip.IfNot(_fixture.IsAvailable, "Docker not available");
        _fixture.Context.Tasks.RemoveRange(_fixture.Context.Tasks);
        _fixture.Context.Users.RemoveRange(_fixture.Context.Users);
        _fixture.Context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [SkippableFact]
    public async Task AddAsync_Should_Persist_Task_To_Database()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.Users.Add(user);
        await _fixture.Context.SaveChangesAsync();

        var task = TaskItem.Create("Integration Test Task", "Test Desc", TaskPriority.High);
        task.AssignTo(user);
        _fixture.Context.Tasks.Add(task);
        await _fixture.Context.SaveChangesAsync();

        var retrieved = await _fixture.Context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == task.Id);

        Assert.NotNull(retrieved);
        Assert.Equal("Integration Test Task", retrieved!.Title);
        Assert.Equal(TaskPriority.High, retrieved.Priority);
        Assert.Equal(user.Id, retrieved.UserId);
    }

    [SkippableFact]
    public async Task SoftDelete_Should_Exclude_From_Query_Filter()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "softdelete_user",
            Email = "soft@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.Users.Add(user);
        await _fixture.Context.SaveChangesAsync();

        var task = TaskItem.Create("Task To Delete", "Desc", TaskPriority.Medium);
        task.AssignTo(user);
        _fixture.Context.Tasks.Add(task);
        await _fixture.Context.SaveChangesAsync();

        task.SoftDelete();
        _fixture.Context.Tasks.Update(task);
        await _fixture.Context.SaveChangesAsync();

        var retrieved = await _fixture.Context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == task.Id);

        Assert.Null(retrieved);
    }

    [SkippableFact]
    public async Task ChangeStatus_Should_Update_Status_In_Database()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "status_user",
            Email = "status@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.Users.Add(user);
        await _fixture.Context.SaveChangesAsync();

        var task = TaskItem.Create("Status Test", "Desc", TaskPriority.Low);
        task.AssignTo(user);
        _fixture.Context.Tasks.Add(task);
        await _fixture.Context.SaveChangesAsync();

        task.ChangeStatus(TaskStatus.InProgress);
        _fixture.Context.Tasks.Update(task);
        await _fixture.Context.SaveChangesAsync();

        var retrieved = await _fixture.Context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == task.Id);

        Assert.NotNull(retrieved);
        Assert.Equal(TaskStatus.InProgress, retrieved!.Status);
    }
}
