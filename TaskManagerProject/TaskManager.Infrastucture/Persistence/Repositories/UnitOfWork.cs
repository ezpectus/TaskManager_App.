using System.Threading;
using System.Threading.Tasks;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastucture.Persistence.Contexts;

namespace TaskManager.Infrastucture.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);
}
