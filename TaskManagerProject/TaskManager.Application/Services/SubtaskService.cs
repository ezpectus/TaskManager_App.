using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Application.DTOs.Subtasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class SubtaskService : ISubtaskService
{
    private readonly ISubtaskRepository _repo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public SubtaskService(ISubtaskRepository repo, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateAsync(CreateSubtaskRequest dto, CancellationToken ct)
    {
        var entity = Subtask.Create(dto.Title, dto.TaskId);

        await _repo.AddAsync(entity, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<SubtaskDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        return e == null ? null : _mapper.Map<SubtaskDto>(e);
    }

    public async Task<IEnumerable<SubtaskDto>> GetAllByTaskIdAsync(Guid taskId, CancellationToken ct)
    {
        var list = await _repo.GetByTaskIdAsync(taskId, ct);
        return _mapper.Map<IEnumerable<SubtaskDto>>(list);
    }

    public async Task<bool> UpdateAsync(Guid id, SubtaskDto dto, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e == null) return false;

        e.Rename(dto.Title);
        if (dto.IsCompleted) e.Complete(); else e.Reopen();

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
