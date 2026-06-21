using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Application.Interfaces;

public interface ITaskTagService
{
    Task<bool> AddTagToTaskAsync(Guid taskId, Guid tagId, CancellationToken ct);
    Task<bool> RemoveTagFromTaskAsync(Guid taskId, Guid tagId, CancellationToken ct);
    Task<IEnumerable<Guid>> GetTagsForTaskAsync(Guid taskId, CancellationToken ct);
}

