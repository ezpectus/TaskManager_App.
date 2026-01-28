using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Application.DTOs.Subtasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Application.Interfaces;
//updated 05.01.26

// Application/Services/SubtaskService.cs


namespace TaskManager.Application.Services;



public class SubtaskService : ISubtaskService
{
    private readonly ISubtaskRepository _repo;
    private readonly IMapper _mapper;

    public SubtaskService(ISubtaskRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateSubtaskRequest dto, CancellationToken ct)
    {
        var entity = new Subtask
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            TaskId = dto.TaskId,
            IsCompleted = false
        };

        await _repo.AddAsync(entity, ct);
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

        e.Title = dto.Title;
        e.IsCompleted = dto.IsCompleted;

        await _repo.UpdateAsync(e, ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var e = await _repo.GetByIdAsync(id, ct);
        if (e == null) return false;

        await _repo.DeleteAsync(e, ct);
        return true;
    }
}
