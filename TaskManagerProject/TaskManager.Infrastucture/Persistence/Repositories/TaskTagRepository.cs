using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastucture.Persistence.Contexts;

namespace TaskManager.Infrastucture.Persistence.Repositories;

public class TaskTagRepository : ITaskTagRepository
{
    private readonly ApplicationDbContext _context;

    public TaskTagRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<bool> ExistsAsync(Guid taskId, Guid tagId, CancellationToken ct)
        => await _context.TaskTags
            .AnyAsync(x => x.TaskId == taskId && x.TagId == tagId, ct);

    public async Task AddAsync(TaskTag entity, CancellationToken ct)
        => await _context.TaskTags.AddAsync(entity, ct);

    public async Task<bool> RemoveAsync(Guid taskId, Guid tagId, CancellationToken ct)
    {
        var entity = await _context.TaskTags
            .FirstOrDefaultAsync(x => x.TaskId == taskId && x.TagId == tagId, ct);

        if (entity == null) return false;

        _context.TaskTags.Remove(entity);
        return true;
    }

    public async Task<IReadOnlyCollection<Tag>> GetTagsByTaskIdAsync(Guid taskId, CancellationToken ct)
        => await _context.TaskTags
            .Where(x => x.TaskId == taskId)
            .Select(x => x.Tag)
            .ToListAsync(ct);
}
