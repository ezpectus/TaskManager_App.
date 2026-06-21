using System;

namespace TaskManager.Domain.Entities;

public class Subtask
{
    public Guid Id { get; set; }
    public string Title { get; private set; } = null!;
    public bool IsCompleted { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;

    public static Subtask Create(string title, Guid taskId)
    {
        return new Subtask
        {
            Id = Guid.NewGuid(),
            Title = title,
            TaskId = taskId,
            IsCompleted = false
        };
    }

    public void Rename(string title)
    {
        Title = title;
    }

    public void Complete()
    {
        IsCompleted = true;
    }

    public void Reopen()
    {
        IsCompleted = false;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}
