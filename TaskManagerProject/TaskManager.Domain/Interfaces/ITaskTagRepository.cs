using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
//updated 26.01.26

namespace TaskManager.Domain.Interfaces
{
    public interface ITaskTagRepository
    {
        Task<bool> ExistsAsync(Guid taskId, Guid tagId, CancellationToken ct);
        Task AddAsync(TaskTag entity, CancellationToken ct);
        Task<bool> RemoveAsync(Guid taskId, Guid tagId, CancellationToken ct);
        Task<IReadOnlyCollection<Tag>> GetTagsByTaskIdAsync(Guid taskId, CancellationToken ct);
    }


}

