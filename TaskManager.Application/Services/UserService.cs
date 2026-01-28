using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Roles;
using TaskManager.Application.DTOs.Users;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
//using TaskManager.Persistence.Repositories;
//using BCrypt.Net;


//updated 05.01.26
// Application/Services/UserService.cs
namespace TaskManager.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> CreateAsync(UserDto dto, CancellationToken ct)
    {
        if (await _repo.ExistsByEmailAsync(dto.Email, ct))
            throw new InvalidOperationException("Email already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = "TEMP", // потом auth сервис
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(user, ct);
        return user.Id;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            Roles = user.UserRoles
                .Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    RoleName = ur.Role.RoleName
                })
                .ToList()
        };
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct)
    {
        var users = await _repo.GetAllAsync(ct);

        return users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            CreatedAt = u.CreatedAt,
            Roles = u.UserRoles
                .Select(ur => new RoleDto
                {
                    Id = ur.Role.Id,
                    RoleName = ur.Role.RoleName
                })
                .ToList()
        });
    }

    public async Task<bool> UpdateAsync(Guid id, UserDto dto, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user == null) return false;

        user.Username = dto.Username;
        user.Email = dto.Email;

        await _repo.UpdateAsync(user, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user == null) return false;

        await _repo.DeleteAsync(user, ct);
        return true;
    }
}




// Alternative implementation with BCrypt

/*
namespace TaskManager.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateAsync(UserCreateDto dto, CancellationToken ct)
    {
        // Проверка уникальности email
        var existing = await _repository.GetByEmailAsync(dto.Email, ct);
        if (existing != null)
            throw new InvalidOperationException("Email already exists");

        // Хэширование пароля
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email.Trim(),
            Username = dto.Username.Trim(),
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(user, ct);
        return user.Id;
    }

    public async Task<UserReadDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        if (user == null) return null;

        return new UserReadDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<IEnumerable<UserReadDto>> GetAllAsync(CancellationToken ct)
    {
        var users = await _repository.GetAllAsync(ct);

        return users.Select(u => new UserReadDto
        {
            Id = u.Id,
            Email = u.Email,
            Username = u.Username,
            CreatedAt = u.CreatedAt
        });
    }

    public async Task<bool> UpdateAsync(Guid id, UserUpdateDto dto, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        if (user == null) return false;

        // Проверка уникальности email если меняется
        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email != user.Email)
        {
            var existing = await _repository.GetByEmailAsync(dto.Email, ct);
            if (existing != null)
                throw new InvalidOperationException("Email already exists");

            user.Email = dto.Email.Trim();
        }

        if (!string.IsNullOrWhiteSpace(dto.Username))
            user.Username = dto.Username.Trim();

        if (!string.IsNullOrWhiteSpace(dto.Password))
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        await _repository.UpdateAsync(user, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var user = await _repository.GetByIdAsync(id, ct);
        if (user == null) return false;

        await _repository.DeleteAsync(id, ct);
        return true;
    }
}
 */