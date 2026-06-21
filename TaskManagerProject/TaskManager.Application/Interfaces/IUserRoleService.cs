using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.Application.Interfaces;

public interface IUserRoleService
{
    Task<bool> AssignRoleAsync(Guid userId, Guid roleId, CancellationToken ct);
    Task<bool> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken ct);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken ct);
}


