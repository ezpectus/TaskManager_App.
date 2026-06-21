using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

public interface IFileAttachmentRepository
{
    Task<FileAttachment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<FileAttachment>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<IEnumerable<FileAttachment>> GetByUserIdAsync(Guid userId, CancellationToken ct);

    Task AddAsync(FileAttachment attachment, CancellationToken ct);
    Task DeleteAsync(FileAttachment attachment, CancellationToken ct);
}


