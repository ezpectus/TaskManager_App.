using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
//updated 05.01.26

namespace TaskManager.Domain.Interfaces;

public interface IActivityLogRepository
{
    Task<IEnumerable<ActivityLog>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<IEnumerable<ActivityLog>> GetByUserIdAsync(Guid userId, CancellationToken ct);
    Task<ActivityLog?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(ActivityLog log, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}



/*
    public interface IActivityLogRepository
    {
        Task<IEnumerable<ActivityLog>> GetByTaskIdAsync(Guid taskId);
        Task<IEnumerable<ActivityLog>> GetByUserIdAsync(Guid userId);
        Task<ActivityLog?> GetByIdAsync(Guid id);
        Task AddAsync(ActivityLog log);
        Task DeleteAsync(Guid id);
    }



 */