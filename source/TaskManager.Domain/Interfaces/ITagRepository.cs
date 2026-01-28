using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
//updated 05.01.26

namespace TaskManager.Domain.Interfaces;

    public interface ITagRepository
    {
        Task<Tag?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<IReadOnlyCollection<Tag>> GetAllAsync(CancellationToken ct);
        Task AddAsync(Tag tag, CancellationToken ct);
        Task DeleteAsync(Tag tag, CancellationToken ct);
    }




