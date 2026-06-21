using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Roles;

namespace TaskManager.Application.Interfaces;

public interface IRoleService
{
    Task<Guid> CreateAsync(RoleDto dto, CancellationToken ct);
    Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, RoleDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}

