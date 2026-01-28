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

public class TaskTagRepository : ITaskTagRepository
{
    private readonly ApplicationDbContext _context;

    public TaskTagRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TaskTag entity)
    {
        await _context.TaskTags.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(Guid taskId, Guid tagId)
    {
        var entity = await _context.TaskTags
            .FirstOrDefaultAsync(x => x.TaskId == taskId && x.TagId == tagId);

        if (entity != null)
        {
            _context.TaskTags.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Tag>> GetTagsByTaskIdAsync(Guid taskId)
    {
        return await _context.TaskTags
            .Where(x => x.TaskId == taskId)
            .Select(x => x.Tag)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByTagIdAsync(Guid tagId)
    {
        return await _context.TaskTags
            .Where(x => x.TagId == tagId)
            .Select(x => x.Task)
            .ToListAsync();
    }
}
