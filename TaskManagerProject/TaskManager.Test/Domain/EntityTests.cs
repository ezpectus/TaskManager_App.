using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Test.Domain;

public class TaskItemTests
{
    [Fact]
    public void Create_Should_Set_Default_Values()
    {
        var task = TaskItem.Create("Title", "Desc", TaskPriority.High);

        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal("Title", task.Title);
        Assert.Equal("Desc", task.Description);
        Assert.Equal(TaskManager.Domain.Enums.TaskStatus.Todo, task.Status);
        Assert.Equal(TaskPriority.High, task.Priority);
        Assert.False(task.IsDeleted);
    }

    [Fact]
    public void Create_Should_Set_Deadline_To_MinValue_When_Not_Provided()
    {
        var task = TaskItem.Create("Title", "Desc", TaskPriority.High);

        Assert.Equal(DateTime.MinValue, task.Deadline);
    }

    [Fact]
    public void Create_Should_Set_Deadline_When_Provided()
    {
        var deadline = new DateTime(2026, 12, 31);
        var task = TaskItem.Create("Title", "Desc", TaskPriority.High, deadline);

        Assert.Equal(deadline, task.Deadline);
    }

    [Fact]
    public void UpdateDetails_Should_Change_Title_And_Description()
    {
        var task = TaskItem.Create("Old", "Old Desc", TaskPriority.Low);

        task.UpdateDetails("New", "New Desc", TaskPriority.High);

        Assert.Equal("New", task.Title);
        Assert.Equal("New Desc", task.Description);
        Assert.Equal(TaskPriority.High, task.Priority);
    }

    [Fact]
    public void UpdateDetails_Should_Update_Deadline_When_Provided()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);
        var newDeadline = new DateTime(2026, 12, 31);

        task.UpdateDetails("T", "D", TaskPriority.Medium, newDeadline);

        Assert.Equal(newDeadline, task.Deadline);
    }

    [Fact]
    public void UpdateDetails_Should_Preserve_Deadline_When_Not_Provided()
    {
        var deadline = new DateTime(2026, 12, 31);
        var task = TaskItem.Create("T", "D", TaskPriority.Medium, deadline);

        task.UpdateDetails("T2", "D2", TaskPriority.High);

        Assert.Equal(deadline, task.Deadline);
    }

    [Fact]
    public void UpdateDetails_Should_Update_UpdatedAt()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);
        var originalUpdatedAt = task.UpdatedAt;

        Thread.Sleep(10);
        task.UpdateDetails("T2", "D2", TaskPriority.High);

        Assert.True(task.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void Create_With_PastDeadline_Should_Store_Correctly()
    {
        var pastDeadline = DateTime.UtcNow.AddDays(-5);
        var task = TaskItem.Create("T", "D", TaskPriority.High, pastDeadline);

        Assert.Equal(pastDeadline, task.Deadline);
    }

    [Fact]
    public void Create_With_FutureDeadline_Should_Store_Correctly()
    {
        var futureDeadline = DateTime.UtcNow.AddDays(30);
        var task = TaskItem.Create("T", "D", TaskPriority.Low, futureDeadline);

        Assert.Equal(futureDeadline, task.Deadline);
    }

    [Fact]
    public void UpdateDetails_With_NullDeadline_Should_Keep_Existing()
    {
        var originalDeadline = DateTime.UtcNow.AddDays(10);
        var task = TaskItem.Create("T", "D", TaskPriority.Medium, originalDeadline);

        task.UpdateDetails("T2", "D2", TaskPriority.High, null);

        Assert.Equal(originalDeadline, task.Deadline);
    }

    [Fact]
    public void ChangeStatus_Should_Update_UpdatedAt()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);
        var originalUpdatedAt = task.UpdatedAt;

        Thread.Sleep(10);
        task.ChangeStatus(TaskManager.Domain.Enums.TaskStatus.InProgress);

        Assert.True(task.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void AssignTo_Should_Set_UserId()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);
        var user = new User { Id = Guid.NewGuid(), Username = "testuser", Email = "test@example.com", PasswordHash = "hash" };

        task.AssignTo(user);

        Assert.Equal(user.Id, task.UserId);
        Assert.NotNull(task.User);
    }

    [Fact]
    public void Touch_Should_Update_UpdatedAt()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);
        var originalUpdatedAt = task.UpdatedAt;

        Thread.Sleep(10);
        task.Touch();

        Assert.True(task.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void ChangeStatus_Should_Update_Status()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);

        task.ChangeStatus(TaskManager.Domain.Enums.TaskStatus.InProgress);

        Assert.Equal(TaskManager.Domain.Enums.TaskStatus.InProgress, task.Status);
    }

    [Fact]
    public void ChangeStatus_Should_Throw_When_ReopeningCompletedTask()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);
        task.MarkAsCompleted();

        Assert.Throws<InvalidOperationException>(() => task.ChangeStatus(TaskManager.Domain.Enums.TaskStatus.Todo));
    }

    [Fact]
    public void MarkAsCompleted_Should_Set_Status_To_Done()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);

        task.MarkAsCompleted();

        Assert.Equal(TaskManager.Domain.Enums.TaskStatus.Done, task.Status);
    }

    [Fact]
    public void SoftDelete_Should_Set_IsDeleted_And_DeletedAt()
    {
        var task = TaskItem.Create("T", "D", TaskPriority.Medium);

        task.SoftDelete();

        Assert.True(task.IsDeleted);
        Assert.NotNull(task.DeletedAt);
    }
}

public class SubtaskTests
{
    [Fact]
    public void Create_Should_Set_Default_Values()
    {
        var taskId = Guid.NewGuid();
        var subtask = Subtask.Create("My Subtask", taskId);

        Assert.NotEqual(Guid.Empty, subtask.Id);
        Assert.Equal("My Subtask", subtask.Title);
        Assert.Equal(taskId, subtask.TaskId);
        Assert.False(subtask.IsCompleted);
    }

    [Fact]
    public void Complete_Should_Set_IsCompleted_True()
    {
        var subtask = Subtask.Create("S", Guid.NewGuid());

        subtask.Complete();

        Assert.True(subtask.IsCompleted);
    }

    [Fact]
    public void Reopen_Should_Set_IsCompleted_False()
    {
        var subtask = Subtask.Create("S", Guid.NewGuid());
        subtask.Complete();

        subtask.Reopen();

        Assert.False(subtask.IsCompleted);
    }

    [Fact]
    public void Rename_Should_Change_Title()
    {
        var subtask = Subtask.Create("Old", Guid.NewGuid());

        subtask.Rename("New");

        Assert.Equal("New", subtask.Title);
    }

    [Fact]
    public void SoftDelete_Should_Set_IsDeleted()
    {
        var subtask = Subtask.Create("S", Guid.NewGuid());

        subtask.SoftDelete();

        Assert.True(subtask.IsDeleted);
        Assert.NotNull(subtask.DeletedAt);
    }
}

public class CommentTests
{
    [Fact]
    public void Create_Should_Set_Values()
    {
        var taskId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        var comment = Comment.Create("Hello", taskId, userId);

        Assert.NotEqual(Guid.Empty, comment.Id);
        Assert.Equal("Hello", comment.Content);
        Assert.Equal(taskId, comment.TaskId);
        Assert.Equal(userId, comment.UserId);
        Assert.False(comment.IsDeleted);
    }

    [Fact]
    public void UpdateContent_Should_Change_Content()
    {
        var comment = Comment.Create("Old", Guid.NewGuid());

        comment.UpdateContent("New");

        Assert.Equal("New", comment.Content);
    }

    [Fact]
    public void SoftDelete_Should_Set_IsDeleted()
    {
        var comment = Comment.Create("Test", Guid.NewGuid());

        comment.SoftDelete();

        Assert.True(comment.IsDeleted);
        Assert.NotNull(comment.DeletedAt);
    }
}
