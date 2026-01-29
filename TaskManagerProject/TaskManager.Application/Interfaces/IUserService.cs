using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs.Users;

//updated 05.01.26
namespace TaskManager.Application.Interfaces;

public interface IUserService
{
    Task<Guid> CreateAsync(UserDto dto, CancellationToken ct);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, UserDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}



/*public interface IUserService
{
    Task<UserDto> GetByIdAsync(Guid id);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task UpdateAsync(Guid id, UpdateUserDto dto);
    Task DeleteAsync(Guid id);
    Task<bool> ValidateCredentialsAsync(string username, string password);
}
 */