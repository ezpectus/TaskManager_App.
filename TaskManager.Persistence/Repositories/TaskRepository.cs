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



public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskItem task, CancellationToken ct)
    {
        await _context.Tasks.AddAsync(task, ct);
    }

    public async Task<List<TaskItem>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Tasks.ToListAsync(ct);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task UpdateAsync(TaskItem task, CancellationToken ct)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync(ct);
    }

    public async Task RemoveAsync(TaskItem task, CancellationToken ct)
    {
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync(ct);
    }
}



/*
public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskItem task, CancellationToken ct)
    {
        await _context.Set<TaskItem>().AddAsync(task, ct);
    }

    public async Task<List<TaskItem>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Set<TaskItem>().ToListAsync(ct);
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Set<TaskItem>().FindAsync([id, ct], ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}
 
*/