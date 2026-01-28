using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Application.Interfaces;
using AutoMapper;
using TaskManager.Application.DTOs.Comments;
//updated 05.01.26


// Application/Services/CommentService.cs
namespace TaskManager.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repo;
    private readonly IMapper _mapper;

    public CommentService(ICommentRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateCommentRequest dto, CancellationToken ct)
    {
        var entity = new Comment
        {
            Id = Guid.NewGuid(),
            Content = dto.Content,
            CreatedAt = DateTime.UtcNow,
            TaskId = dto.TaskId,
            UserId = dto.UserId
        };

        await _repo.AddAsync(entity, ct);
        return entity.Id;
    }

    public async Task<CommentDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        return e == null ? null : _mapper.Map<CommentDto>(e);
    }

    public async Task<IEnumerable<CommentDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
    {
        var list = await _repo.GetByTaskIdAsync(taskId, ct);
        return _mapper.Map<IEnumerable<CommentDto>>(list);
    }

    public async Task<bool> UpdateAsync(Guid id, CreateCommentRequest dto, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e == null) return false;

        e.Content = dto.Content;
        await _repo.UpdateAsync(e, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e == null) return false;

        await _repo.DeleteAsync(id, ct);
        return true;
    }
}
