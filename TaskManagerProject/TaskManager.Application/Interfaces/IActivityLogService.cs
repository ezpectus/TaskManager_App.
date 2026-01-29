using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26


// Application/Interfaces/IActivityLogService.cs
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs.Activity;


namespace TaskManager.Application.Interfaces;

public interface IActivityLogService
{
    Task<IEnumerable<ActivityLogReadDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<IEnumerable<ActivityLogReadDto>> GetByUserIdAsync(Guid userId, CancellationToken ct);
}

