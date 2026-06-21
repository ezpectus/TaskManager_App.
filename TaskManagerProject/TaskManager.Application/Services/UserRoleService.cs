using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly IUserRoleRepository _userRoleRepo;
    private readonly IUnitOfWork _unitOfWork;

    public UserRoleService(
        IUserRepository userRepo,
        IRoleRepository roleRepo,
        IUserRoleRepository userRoleRepo,
        IUnitOfWork unitOfWork)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _userRoleRepo = userRoleRepo;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AssignRoleAsync(Guid userId, Guid roleId, CancellationToken ct)
    {
        var user = await _userRepo.GetByIdAsync(userId, ct);
        if (user == null) return false;

        var role = await _roleRepo.GetByIdAsync(roleId, ct);
        if (role == null) return false;

        var existing = await _userRoleRepo.GetUserRoleAsync(userId, roleId, ct);
        if (existing != null) return false;

        await _userRoleRepo.AddAsync(new UserRole
        {
            UserId = userId,
            RoleId = roleId
        }, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> RemoveRoleAsync(Guid userId, Guid roleId, CancellationToken ct)
    {
        var existing = await _userRoleRepo.GetUserRoleAsync(userId, roleId, ct);
        if (existing == null) return false;

        await _userRoleRepo.RemoveAsync(existing, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId, CancellationToken ct)
    {
        var roles = await _userRoleRepo.GetByUserIdAsync(userId, ct);
        return roles.Select(x => x.Role.RoleName);
    }
}
