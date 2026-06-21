using System;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public class UpdateTaskRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public DateTime? Deadline { get; set; }
}
