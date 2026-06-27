using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using TaskManager.Application.DTOs.Auth;
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
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository repo, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
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
        _logger.LogInformation("User registered: '{Username}' ({Email}, Id: {UserId})", user.Username, user.Email, user.Id);
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
        _logger.LogInformation("User deleted: '{Username}' ({Email}, Id: {UserId})", user.Username, user.Email, user.Id);
        return true;
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken ct)
    {
        var user = await _repo.GetByEmailAsync(email, ct);
        if (user == null) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(request.UserId, ct);
        if (user == null) return false;

        if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            return false;

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        await _repo.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Password changed for user '{Username}' (Id: {UserId})", user.Username, user.Id);
        return true;
    }

    public async Task<bool> UpdateProfileAsync(Guid id, UpdateUserRequest dto, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(id, ct);
        if (user == null) return false;

        user.Username = dto.Username.Trim();
        user.Email = dto.Email.Trim();

        await _repo.UpdateAsync(user, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}