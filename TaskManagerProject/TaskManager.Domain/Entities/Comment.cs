using System;

namespace TaskManager.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; private set; } = null!;
    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;

    public Guid? UserId { get; set; }
    public User? User { get; set; }

    public static Comment Create(string content, Guid taskId, Guid? userId = null)
    {
        return new Comment
        {
            Id = Guid.NewGuid(),
            Content = content,
            CreatedAt = DateTime.UtcNow,
            TaskId = taskId,
            UserId = userId
        };
    }

    public void UpdateContent(string content)
    {
        Content = content;
    }

    public void SoftDelete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}
