using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Tags;

namespace TaskManager.Application.Interfaces;

public interface ITagService
{
    Task<Guid> CreateAsync(TagDto dto, CancellationToken ct);
    Task<TagDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<TagDto>> GetAllAsync(CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}


