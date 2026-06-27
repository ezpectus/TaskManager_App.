using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence.Contexts;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
        => _context = context;

    public async Task AddAsync(TaskItem task, CancellationToken ct)
        => await _context.Tasks.AddAsync(task, ct);

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Tasks
            .Include(t => t.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<IReadOnlyCollection<TaskItem>> GetAllAsync(CancellationToken ct)
        => await _context.Tasks
            .Include(t => t.User)
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Take(500)
            .ToListAsync(ct);

    public async Task<(IReadOnlyCollection<TaskItem> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize,
        Domain.Enums.TaskStatus? status = null,
        TaskPriority? priority = null,
        Guid? userId = null,
        string? searchTerm = null,
        CancellationToken ct = default)
    {
        var query = _context.Tasks
            .Include(t => t.User)
            .AsNoTracking()
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(t => t.Status == status.Value);

        if (priority.HasValue)
            query = query.Where(t => t.Priority == priority.Value);

        if (userId.HasValue)
            query = query.Where(t => t.UserId == userId.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm));

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public Task UpdateAsync(TaskItem task, CancellationToken ct)
    {
        _context.Tasks.Update(task);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TaskItem task, CancellationToken ct)
    {
        _context.Tasks.Remove(task);
        return Task.CompletedTask;
    }
}
