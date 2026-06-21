using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IUserRoleRepository
{
    Task<UserRole?> GetUserRoleAsync(Guid userId, Guid roleId, CancellationToken ct);
    Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId, CancellationToken ct);

    Task AddAsync(UserRole userRole, CancellationToken ct);
    Task RemoveAsync(UserRole userRole, CancellationToken ct);
}
