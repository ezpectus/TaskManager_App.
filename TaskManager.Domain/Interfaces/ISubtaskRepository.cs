using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

//updated 05.01.26

namespace TaskManager.Domain.Interfaces;

    public interface ISubtaskRepository
    {
        Task<Subtask?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Subtask>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
        Task AddAsync(Subtask subtask, CancellationToken ct);
        Task UpdateAsync(Subtask subtask, CancellationToken ct);
        Task DeleteAsync(Subtask subtask, CancellationToken ct);
    }



