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


namespace TaskManager.Infrastucture.Persistence.Repositories;


public class SubtaskRepository : ISubtaskRepository
{
    private readonly ApplicationDbContext _context;

    public SubtaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Subtask?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Subtasks
            .FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IEnumerable<Subtask>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
        => await _context.Subtasks
            .Where(s => s.TaskId == taskId)
            .ToListAsync(ct);

    public async Task AddAsync(Subtask subtask, CancellationToken ct)
    {
        await _context.Subtasks.AddAsync(subtask, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Subtask subtask, CancellationToken ct)
    {
        _context.Subtasks.Update(subtask);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Subtask subtask, CancellationToken ct)
    {
        _context.Subtasks.Remove(subtask);
        await _context.SaveChangesAsync(ct);
    }
}
