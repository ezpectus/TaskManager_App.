using TaskManager.Application.DTOs.Common;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.DTOs.Tasks;

public class TaskFilterRequest : PaginationRequest
{
    public Domain.Enums.TaskStatus? Status { get; set; }
    public TaskPriority? Priority { get; set; }
    public Guid? UserId { get; set; }
    public string? SearchTerm { get; set; }
}
