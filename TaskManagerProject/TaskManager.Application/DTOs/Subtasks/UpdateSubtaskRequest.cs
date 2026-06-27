namespace TaskManager.Application.DTOs.Subtasks;

public class UpdateSubtaskRequest
{
    public string? Title { get; set; }
    public bool? IsCompleted { get; set; }
}
