using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Tasks;

namespace TaskManager.Application.Interfaces;

public interface ITaskService
{
    Task<Guid> CreateAsync(CreateTaskRequest dto, CancellationToken ct);
    Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken ct);
    Task<PagedResult<TaskDto>> GetPagedAsync(TaskFilterRequest filter, CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, UpdateTaskRequest dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<bool> AssignAsync(Guid taskId, Guid userId, CancellationToken ct);
}
