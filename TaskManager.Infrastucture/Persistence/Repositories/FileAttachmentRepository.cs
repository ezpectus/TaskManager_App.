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


public class FileAttachmentRepository : IFileAttachmentRepository
{
    private readonly ApplicationDbContext _context;

    public FileAttachmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FileAttachment?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Attachments
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IEnumerable<FileAttachment>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
        => await _context.Attachments
            .Include(a => a.User)
            .Where(a => a.TaskId == taskId)
            .ToListAsync(ct);

    public async Task<IEnumerable<FileAttachment>> GetByUserIdAsync(Guid userId, CancellationToken ct)
        => await _context.Attachments
            .Where(a => a.UserId == userId)
            .ToListAsync(ct);

    public async Task AddAsync(FileAttachment attachment, CancellationToken ct)
    {
        await _context.Attachments.AddAsync(attachment, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(FileAttachment attachment, CancellationToken ct)
    {
        _context.Attachments.Remove(attachment);
        await _context.SaveChangesAsync(ct);
    }
}
