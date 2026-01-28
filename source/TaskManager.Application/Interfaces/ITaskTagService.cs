using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
//updated 26.01.26

// Application/Interfaces/ITaskTagService.cs
namespace TaskManager.Application.Interfaces;

public interface ITaskTagService
{
    Task<bool> AddTagToTaskAsync(Guid taskId, Guid tagId, CancellationToken ct);
    Task<bool> RemoveTagFromTaskAsync(Guid taskId, Guid tagId, CancellationToken ct);
    Task<IEnumerable<Guid>> GetTagsForTaskAsync(Guid taskId, CancellationToken ct);
}

