using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Attachments;

namespace TaskManager.Application.Interfaces;

public interface IFileAttachmentService
{
    Task<Guid> CreateAsync(CreateAttachmentRequest dto, CancellationToken ct);
    Task<FileAttachmentDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<FileAttachmentDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}


