using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using TaskManager.Application.DTOs.Users;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateUserRequest dto, CancellationToken ct)
    {
        if (await _repo.ExistsByEmailAsync(dto.Email, ct))
            throw new InvalidOperationException("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username.Trim(),
            Email = dto.Email.Trim(),
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return user.Id;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct)
    {
        var users = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<bool> UpdateAsync(Guid id, UserDto dto, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user == null) return false;

        user.Username = dto.Username;
        user.Email = dto.Email;

        await _repo.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user == null) return false;

        await _repo.DeleteAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken ct)
    {
        var user = await _repo.GetByEmailAsync(email, ct);
        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }
}