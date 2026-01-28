using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs.Tags;
//updated 05.01.26


// Application/Interfaces/ITagService.cs

namespace TaskManager.Application.Interfaces;

public interface ITagService
{
    Task<Guid> CreateAsync(TagDto dto, CancellationToken ct);
    Task<TagDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<TagDto>> GetAllAsync(CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}


