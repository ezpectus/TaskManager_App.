using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs.Users;

//updated 05.01.26
// Application/Interfaces/IUserRoleService.cs
namespace TaskManager.Application.Interfaces;

public interface IUserRoleService
{
    Task<bool> AssignRoleAsync(Guid userId, Guid roleId, CancellationToken ct);
    Task<bool> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken ct);
    Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken ct);
}


