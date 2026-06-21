using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IRoleRepository
{
    Task<Role?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct);
    Task AddAsync(Role role, CancellationToken ct);
    Task UpdateAsync(Role role, CancellationToken ct);
    Task DeleteAsync(Role role, CancellationToken ct);
}
