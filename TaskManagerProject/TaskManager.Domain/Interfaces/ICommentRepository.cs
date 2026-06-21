using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface ICommentRepository
{
    Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<Comment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Comment comment, CancellationToken ct);
    Task UpdateAsync(Comment comment, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}

