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

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Comment>> GetByTaskIdAsync(Guid taskId) =>
        await _context.Comments.Where(c => c.TaskId == taskId).ToListAsync();

    public async Task<IEnumerable<Comment>> GetByUserIdAsync(Guid userId) =>
        await _context.Comments.Where(c => c.UserId == userId).ToListAsync();

    public async Task<Comment?> GetByIdAsync(Guid id) =>
        await _context.Comments.FindAsync(id);

    public async Task AddAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Comments.FindAsync(id);
        if (entity != null)
        {
            _context.Comments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}

