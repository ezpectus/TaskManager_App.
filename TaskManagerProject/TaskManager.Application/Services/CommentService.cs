using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Application.DTOs.Comments;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CommentService(ICommentRepository repo, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateAsync(CreateCommentRequest dto, CancellationToken ct)
    {
        var entity = Comment.Create(dto.Content, dto.TaskId, dto.UserId);

        await _repo.AddAsync(entity, ct);
        await _unitOfWork.SaveChangesAsync(ct);
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

    public async Task<bool> UpdateAsync(Guid id, UpdateCommentRequest dto, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e == null) return false;

        e.UpdateContent(dto.Content);
        await _repo.UpdateAsync(e, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e == null) return false;

        e.SoftDelete();
        await _repo.UpdateAsync(e, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
