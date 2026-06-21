using System;

namespace TaskManager.Application.DTOs.Subtasks;

public class SubtaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }

    public Guid TaskId { get; set; }
}
