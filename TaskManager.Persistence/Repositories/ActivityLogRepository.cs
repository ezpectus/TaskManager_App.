using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Persistence.Contexts;



namespace TaskManager.Persistence.Repositories;


public class ActivityLogRepository : IActivityLogRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ActivityLog>> GetByTaskIdAsync(Guid taskId) =>
        await _context.ActivityLogs.Where(x => x.TaskId == taskId).ToListAsync();

    public async Task<IEnumerable<ActivityLog>> GetByUserIdAsync(Guid userId) =>
        await _context.ActivityLogs.Where(x => x.UserId == userId).ToListAsync();

    public async Task<ActivityLog?> GetByIdAsync(Guid id) =>
        await _context.ActivityLogs.FindAsync(id);

    public async Task AddAsync(ActivityLog log)
    {
        await _context.ActivityLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.ActivityLogs.FindAsync(id);
        if (entity != null)
        {
            _context.ActivityLogs.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
