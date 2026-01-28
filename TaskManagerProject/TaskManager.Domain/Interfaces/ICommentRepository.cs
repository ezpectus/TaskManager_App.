using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
//updated 05.01.26

namespace TaskManager.Domain.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Comment comment, CancellationToken ct);
    Task UpdateAsync(Comment comment, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}

