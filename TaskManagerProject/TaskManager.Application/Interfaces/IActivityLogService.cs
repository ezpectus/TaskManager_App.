using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Activity;

namespace TaskManager.Application.Interfaces;

public interface IActivityLogService
{
    Task<IEnumerable<ActivityLogReadDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<IEnumerable<ActivityLogReadDto>> GetByUserIdAsync(Guid userId, CancellationToken ct);
}

