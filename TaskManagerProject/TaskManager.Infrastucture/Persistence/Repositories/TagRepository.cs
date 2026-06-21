using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastucture.Persistence.Contexts;

namespace TaskManager.Infrastucture.Persistence.Repositories;

public class TagRepository : ITagRepository
{
    private readonly ApplicationDbContext _context;

    public TagRepository(ApplicationDbContext context)
        => _context = context;

    public async Task<Tag?> GetByIdAsync(Guid id, CancellationToken ct)
        => await _context.Tags.FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<IReadOnlyCollection<Tag>> GetAllAsync(CancellationToken ct)
        => await _context.Tags.ToListAsync(ct);

    public async Task AddAsync(Tag tag, CancellationToken ct)
        => await _context.Tags.AddAsync(tag, ct);

    public Task DeleteAsync(Tag tag, CancellationToken ct)
    {
        _context.Tags.Remove(tag);
        return Task.CompletedTask;
    }
}
