using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastucture.Persistence.Contexts;

//updated 05.01.26
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
    {
        await _context.Roles.AddAsync(role, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Role role, CancellationToken ct)
    {
        _context.Roles.Update(role);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Role role, CancellationToken ct)
    {
        _context.Roles.Remove(role);
        await _context.SaveChangesAsync(ct);
    }
}
