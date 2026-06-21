using System;

namespace TaskManager.Application.DTOs.Subtasks;

public class CreateSubtaskRequest
{
    public string Title { get; set; } = null!;
    public Guid TaskId { get; set; }
}
