using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IActivityLogRepository
{
    Task<IEnumerable<ActivityLog>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<IEnumerable<ActivityLog>> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task<ActivityLog?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(ActivityLog log, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}
