using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<TaskService> _logger;

    public TaskService(ITaskRepository repo, IUserRepository userRepo, IMapper mapper, IUnitOfWork unitOfWork, ILogger<TaskService> logger)
    {
        _repo = repo;
        _userRepo = userRepo;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(CreateTaskRequest dto, CancellationToken ct)
    {
        var deadline = dto.Deadline;
        var entity = TaskItem.Create(dto.Title, dto.Description, dto.Priority, deadline);

        if (dto.UserId.HasValue)
            entity.UserId = dto.UserId;

        await _repo.AddAsync(entity, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Task created: '{Title}' (Id: {TaskId}, Priority: {Priority})", entity.Title, entity.Id, entity.Priority);
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
        var task = await _repo.GetByIdForUpdateAsync(id, ct);
        if (task == null) return false;

        if (dto.Title != null || dto.Description != null || dto.Priority.HasValue || dto.Deadline.HasValue)
        {
            task.UpdateDetails(
                dto.Title ?? task.Title,
                dto.Description ?? task.Description,
                dto.Priority ?? task.Priority,
                dto.Deadline);
        }

        if (dto.Status.HasValue && dto.Status.Value != task.Status)
        {
            try
            {
                task.ChangeStatus(dto.Status.Value);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        task.Touch();

        await _repo.UpdateAsync(task, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var task = await _repo.GetByIdForUpdateAsync(id, ct);
        if (task == null) return false;

        task.SoftDelete();
        await _repo.UpdateAsync(task, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Task soft-deleted: '{Title}' (Id: {TaskId})", task.Title, task.Id);
        return true;
    }

    public async Task<bool> AssignAsync(Guid taskId, Guid userId, CancellationToken ct)
    {
        var task = await _repo.GetByIdForUpdateAsync(taskId, ct);
        if (task == null) return false;

        var user = await _userRepo.GetByIdAsync(userId, ct);
        if (user == null) return false;

        task.AssignTo(user);
        await _repo.UpdateAsync(task, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Task '{Title}' (Id: {TaskId}) assigned to user '{Username}' (Id: {UserId})", task.Title, task.Id, user.Username, user.Id);
        return true;
    }
}