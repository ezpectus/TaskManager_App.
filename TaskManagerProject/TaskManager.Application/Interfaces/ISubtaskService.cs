using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Subtasks;

namespace TaskManager.Application.Interfaces;

public interface ISubtaskService
{
    Task<Guid> CreateAsync(CreateSubtaskRequest dto, CancellationToken ct);
    Task<SubtaskDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<SubtaskDto>> GetAllByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, SubtaskDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}
