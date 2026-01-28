using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Roles;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

//updated 05.01.26

// Application/Services/RoleService.cs
namespace TaskManager.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repo;

    public RoleService(IRoleRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> CreateAsync(RoleDto dto, CancellationToken ct)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            RoleName = dto.RoleName
        };

        await _repo.AddAsync(role, ct);
        return role.Id;
    }

    public async Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(id, ct);
        if (role == null) return null;

        return new RoleDto
        {
            Id = role.Id,
            RoleName = role.RoleName
        };
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken ct)
    {
        var roles = await _repo.GetAllAsync(ct);
        return roles.Select(r => new RoleDto
        {
            Id = r.Id,
            RoleName = r.RoleName
        });
    }

    public async Task<bool> UpdateAsync(Guid id, RoleDto dto, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(id, ct);
        if (role == null) return false;

        role.RoleName = dto.RoleName;
        await _repo.UpdateAsync(role, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(id, ct);
        if (role == null) return false;

        await _repo.DeleteAsync(role, ct);
        return true;
    }
}
