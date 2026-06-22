using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence.Contexts;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ApplicationDbContext _context;

    public RoleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Role?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await _context.Roles.FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct) =>
        await _context.Roles.ToListAsync(ct);

    public async Task AddAsync(Role role, CancellationToken ct)
        => await _context.Roles.AddAsync(role, ct);

    public Task UpdateAsync(Role role, CancellationToken ct)
    {
        _context.Roles.Update(role);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Role role, CancellationToken ct)
    {
        _context.Roles.Remove(role);
        return Task.CompletedTask;
    }
}
