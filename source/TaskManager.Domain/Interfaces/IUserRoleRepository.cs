using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;


//updated 05.01.26
namespace TaskManager.Domain.Interfaces;

public interface IUserRoleRepository
{
    Task<UserRole?> GetUserRoleAsync(Guid userId, Guid roleId, CancellationToken ct);
    Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId, CancellationToken ct);

    Task AddAsync(UserRole userRole, CancellationToken ct);
    Task RemoveAsync(UserRole userRole, CancellationToken ct);
}



/* Application/Interfaces/IUserRoleRepository.cs

    public interface IUserRoleRepository
    {
        Task<UserRole?> GetUserRoleAsync(Guid userId, Guid roleId);
        Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<UserRole>> GetByRoleIdAsync(Guid roleId);

        Task AddAsync(UserRole userRole);
        Task RemoveAsync(UserRole userRole);

        Task AssignRoleToUserAsync(Guid userId, Guid roleId);
        Task RemoveRoleFromUserAsync(Guid userId, Guid roleId);
    }

 */