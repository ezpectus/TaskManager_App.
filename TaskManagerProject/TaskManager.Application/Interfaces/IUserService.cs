using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Users;

namespace TaskManager.Application.Interfaces;

public interface IUserService
{
    Task<Guid> CreateAsync(CreateUserRequest dto, CancellationToken ct);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, UserDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<bool> ValidateCredentialsAsync(string email, string password, CancellationToken ct);
}