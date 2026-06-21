using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyCollection<Tag>> GetAllAsync(CancellationToken ct);
    Task AddAsync(Tag tag, CancellationToken ct);
    Task DeleteAsync(Tag tag, CancellationToken ct);
}


