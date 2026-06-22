using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Interfaces;

public interface ITaskRepository
{
    Task AddAsync(TaskItem task, CancellationToken ct);
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyCollection<TaskItem>> GetAllAsync(CancellationToken ct);
    Task<(IReadOnlyCollection<TaskItem> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize,
        Domain.Enums.TaskStatus? status = null,
        TaskPriority? priority = null,
        Guid? userId = null,
        string? searchTerm = null,
        CancellationToken ct = default);
    Task UpdateAsync(TaskItem task, CancellationToken ct);
    Task DeleteAsync(TaskItem task, CancellationToken ct);
}



