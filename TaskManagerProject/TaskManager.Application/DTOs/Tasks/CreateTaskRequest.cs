using System;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public class CreateTaskRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public TaskStatus Status { get; set; } = TaskStatus.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime Deadline { get; set; }

    public Guid? UserId { get; set; }
}
