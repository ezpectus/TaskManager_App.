using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repo;
    private readonly IUserRepository _userRepo;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public TaskService(ITaskRepository repo, IUserRepository userRepo, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _userRepo = userRepo;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateAsync(CreateTaskRequest dto, CancellationToken ct)
    {
        var entity = _mapper.Map<TaskItem>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        await _repo.AddAsync(entity, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        return task == null ? null : _mapper.Map<TaskDto>(task);
    }

    public async Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken ct)
    {
        var list = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<TaskDto>>(list);
    }

    public async Task<PagedResult<TaskDto>> GetPagedAsync(TaskFilterRequest filter, CancellationToken ct)
    {
        var (items, totalCount) = await _repo.GetPagedAsync(
            filter.Page, filter.PageSize,
            filter.Status, filter.Priority,
            filter.UserId, filter.SearchTerm, ct);

        return new PagedResult<TaskDto>
        {
            Items = _mapper.Map<IReadOnlyCollection<TaskDto>>(items),
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<bool> UpdateAsync(Guid id, UpdateTaskRequest dto, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task == null) return false;

        _mapper.Map(dto, task);
        task.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(task, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task == null) return false;

        task.SoftDelete();
        await _repo.UpdateAsync(task, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> AssignAsync(Guid taskId, Guid userId, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(taskId, ct);
        if (task == null) return false;

        var user = await _userRepo.GetByIdAsync(userId, ct);
        if (user == null) return false;

        task.AssignTo(user);
        await _repo.UpdateAsync(task, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}