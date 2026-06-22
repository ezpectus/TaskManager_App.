using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence.Contexts;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly ApplicationDbContext _context;

    public UserRoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserRole?> GetUserRoleAsync(Guid userId, Guid roleId,CancellationToken ct)
    {
        return await _context.UserRoles
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId,ct);
    }

    public async Task<IEnumerable<UserRole>> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        return await _context.UserRoles
            .Include(x => x.Role)
            .Where(x => x.UserId == userId)
            .ToListAsync(ct);
    }

    public async Task AddAsync(UserRole userRole, CancellationToken ct)
        => await _context.UserRoles.AddAsync(userRole, ct);

    public Task RemoveAsync(UserRole userRole, CancellationToken ct)
    {
        _context.UserRoles.Remove(userRole);
        return Task.CompletedTask;
    }
}
