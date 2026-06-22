using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence.Contexts;

namespace TaskManager.Infrastructure.Persistence.Repositories;


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
        => await _context.Subtasks.AddAsync(subtask, ct);

    public Task UpdateAsync(Subtask subtask, CancellationToken ct)
    {
        _context.Subtasks.Update(subtask);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Subtask subtask, CancellationToken ct)
    {
        _context.Subtasks.Remove(subtask);
        return Task.CompletedTask;
    }
}
