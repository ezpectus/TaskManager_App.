using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Comments;

namespace TaskManager.Application.Interfaces;

public interface ICommentService
{
    Task<Guid> CreateAsync(CreateCommentRequest dto, CancellationToken ct);
    Task<CommentDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<CommentDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, CreateCommentRequest dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}

