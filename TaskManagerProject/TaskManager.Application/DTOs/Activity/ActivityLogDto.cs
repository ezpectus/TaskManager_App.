using System;

namespace TaskManager.Application.DTOs.Activity;

public class ActivityLogReadDto
{
    public Guid Id { get; set; }
    public string ActionType { get; set; } = null!;
    public DateTime Timestamp { get; set; }

    public Guid TaskId { get; set; }
    public string? TaskTitle { get; set; }

    public Guid? UserId { get; set; }
    public string? Username { get; set; }
}

public class ActivityLogCreateDto
{
    public string ActionType { get; set; } = null!;
    public Guid TaskId { get; set; }
    public Guid? UserId { get; set; }
}
