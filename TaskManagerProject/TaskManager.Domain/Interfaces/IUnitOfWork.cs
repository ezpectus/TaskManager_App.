using System.Threading;
using System.Threading.Tasks;

namespace TaskManager.Domain.Interfaces;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
