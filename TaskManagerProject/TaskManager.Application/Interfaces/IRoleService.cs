using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs.Roles;
//updated 05.01.26

// Application/Interfaces/IRoleService.cs
namespace TaskManager.Application.Interfaces;

public interface IRoleService
{
    Task<Guid> CreateAsync(RoleDto dto, CancellationToken ct);
    Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, RoleDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}

