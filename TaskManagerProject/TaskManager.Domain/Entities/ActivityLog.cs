using System;

namespace TaskManager.Domain.Entities;

public class ActivityLog
{
    public Guid Id { get; set; }

    public string ActionType { get; set; } = null!;
    public DateTime Timestamp { get; set; }

    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}

