using System;

namespace TaskManager.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}