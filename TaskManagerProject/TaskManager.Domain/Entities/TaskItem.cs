using System;
using System.Collections.Generic;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }

    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Domain.Enums.TaskStatus Status { get; private set; } = Domain.Enums.TaskStatus.Todo;
    public TaskPriority Priority { get; private set; } = TaskPriority.Medium;

    public DateTime Deadline { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; private set; }

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public List<Subtask> Subtasks { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<FileAttachment> Attachments { get; set; } = new();
    public List<TaskTag> TaskTags { get; set; } = new();
    public List<ActivityLog> ActivityLogs { get; set; } = new();

    public static TaskItem Create(string title, string description, TaskPriority priority, DateTime? deadline = null)
    {
        return new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Status = Domain.Enums.TaskStatus.Todo,
            Priority = priority,
            Deadline = deadline ?? DateTime.MaxValue,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void UpdateDetails(string title, string description, TaskPriority priority, DateTime? deadline = null)
    {
        Title = title;
        Description = description;
        Priority = priority;
        Deadline = deadline ?? Deadline;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(Domain.Enums.TaskStatus newStatus)
    {
        if (Status == Domain.Enums.TaskStatus.Done && newStatus != Domain.Enums.TaskStatus.Done)
            throw new InvalidOperationException("Completed task cannot be reopened");
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsCompleted()
    {
        Status = Domain.Enums.TaskStatus.Done;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignTo(User user)
    {
        User = user;
        UserId = user.Id;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void Touch()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}