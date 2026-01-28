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

public class SubtaskRepository : ISubtaskRepository
{
    private readonly ApplicationDbContext _context;

    public SubtaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Subtask?> GetByIdAsync(Guid id)
    {
        return await _context.Subtasks
            .Include(s => s.Task)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Subtask>> GetByTaskIdAsync(Guid taskId)
    {
        return await _context.Subtasks
            .Where(s => s.TaskId == taskId)
            .ToListAsync();
    }

    public async Task AddAsync(Subtask subtask)
    {
        await _context.Subtasks.AddAsync(subtask);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Subtask subtask)
    {
        _context.Subtasks.Update(subtask);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Subtasks.FindAsync(id);
        if (entity != null)
        {
            _context.Subtasks.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
