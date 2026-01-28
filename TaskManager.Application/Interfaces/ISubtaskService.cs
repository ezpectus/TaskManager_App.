using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Application.DTOs.Subtasks;

//updated 05.01.26

// Application/Interfaces/ISubtaskService.cs

namespace TaskManager.Application.Interfaces;

public interface ISubtaskService
{
    Task<Guid> CreateAsync(CreateSubtaskRequest dto, CancellationToken ct);
    Task<SubtaskDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<SubtaskDto>> GetAllByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, SubtaskDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}




/*
namespace TaskManager.Application.Interfaces
{
    public interface ISubtaskService
    {
        Task<Subtask?> GetByIdAsync(Guid id);
        Task<IEnumerable<Subtask>> GetByTaskIdAsync(Guid taskId);
        Task AddSubtaskAsync(Guid taskId, string title);
        Task UpdateSubtaskAsync(Subtask subtask);
        Task MarkAsCompletedAsync(Guid subtaskId);
        Task DeleteSubtaskAsync(Guid id);
    }
}
*/