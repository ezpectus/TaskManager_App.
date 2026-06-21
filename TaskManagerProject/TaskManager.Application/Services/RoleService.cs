using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Application.DTOs.Roles;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoleService(IRoleRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(RoleDto dto, CancellationToken ct)
    {
        var role = new Role
        {
            Id = Guid.NewGuid(),
            RoleName = dto.RoleName
        };

        await _repo.AddAsync(role, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return role.Id;
    }

    public async Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(id, ct);
        return role == null ? null : _mapper.Map<RoleDto>(role);
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync(CancellationToken ct)
    {
        var roles = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<RoleDto>>(roles);
    }

    public async Task<bool> UpdateAsync(Guid id, RoleDto dto, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(id, ct);
        if (role == null) return false;

        role.RoleName = dto.RoleName;
        await _repo.UpdateAsync(role, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(id, ct);
        if (role == null) return false;

        await _repo.DeleteAsync(role, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
