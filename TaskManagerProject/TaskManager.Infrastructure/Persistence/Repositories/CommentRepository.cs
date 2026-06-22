using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence.Contexts;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId, CancellationToken ct) =>
        await _context.Comments.Where(c => c.TaskId == taskId).ToListAsync(ct);

    public async Task<Comment?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await _context.Comments.FindAsync([id], ct);

    public async Task AddAsync(Comment comment, CancellationToken ct)
        => await _context.Comments.AddAsync(comment, ct);

    public Task UpdateAsync(Comment comment, CancellationToken ct)
    {
        _context.Comments.Update(comment);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _context.Comments.FindAsync([id], ct);
        if (entity != null)
            _context.Comments.Remove(entity);
    }
}
