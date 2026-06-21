using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastucture.Persistence.Contexts;

namespace TaskManager.Infrastucture.Persistence.Repositories;



public class ActivityLogRepository : IActivityLogRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ActivityLog>> GetByTaskIdAsync(Guid taskId, CancellationToken ct) =>
     await _context.ActivityLogs.Where(x => x.TaskId == taskId).ToListAsync(ct);

    public async Task<IEnumerable<ActivityLog>> GetByUserIdAsync(Guid userId, CancellationToken ct) =>
        await _context.ActivityLogs.Where(x => x.UserId == userId).ToListAsync(ct);

    public async Task<ActivityLog?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await _context.ActivityLogs.FindAsync([id], ct);

    public async Task AddAsync(ActivityLog log, CancellationToken ct)
        => await _context.ActivityLogs.AddAsync(log, ct);

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _context.ActivityLogs.FindAsync([id], ct);
        if (entity != null)
            _context.ActivityLogs.Remove(entity);
    }

}
