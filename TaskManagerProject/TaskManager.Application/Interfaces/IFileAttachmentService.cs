using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs.Attachments;
//updated 05.01.26

// Application/Interfaces/IFileAttachmentService.cs

namespace TaskManager.Application.Interfaces;

public interface IFileAttachmentService
{
    Task<Guid> CreateAsync(CreateAttachmentRequest dto, CancellationToken ct);
    Task<FileAttachmentDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<FileAttachmentDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}


