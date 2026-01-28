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


public class AttachmentRepository : IAttachmentRepository
{
    private readonly ApplicationDbContext _context;

    public AttachmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FileAttachment>> GetByTaskIdAsync(Guid taskId) =>
        await _context.Attachments.Where(a => a.TaskId == taskId).ToListAsync();

    public async Task<IEnumerable<FileAttachment>> GetByUserIdAsync(Guid userId) =>
        await _context.Attachments.Where(a => a.UserId == userId).ToListAsync();

    public async Task<FileAttachment?> GetByIdAsync(Guid id) =>
        await _context.Attachments.FindAsync(id);

    public async Task AddAsync(FileAttachment attachment)
    {
        await _context.Attachments.AddAsync(attachment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Attachments.FindAsync(id);
        if (entity != null)
        {
            _context.Attachments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}


