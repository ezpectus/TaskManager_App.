using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using AutoMapper;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Application.Sorting;
using System.Collections;
using TaskManager.Application.DTOs.Tasks;

// Application/Services/TaskService.cs
//updated 26/01/26

namespace TaskManager.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repo;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(CreateTaskRequest dto, CancellationToken ct)
    {
        var entity = _mapper.Map<TaskItem>(dto);
        entity.Id = Guid.NewGuid();
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = DateTime.UtcNow;

        await _repo.AddAsync(entity, ct);
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


    public async Task<bool> UpdateAsync(Guid id, UpdateTaskRequest dto, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task == null) return false;

        _mapper.Map(dto, task);
        task.UpdatedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(task, ct);
        return true;
    }


    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task == null) return false;

        await _repo.DeleteAsync(task, ct);
        return true;
    }
}



/*
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repo;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Guid> CreateTaskAsync(TaskDtos.TaskCreateDto request, CancellationToken ct)
    {
        var task = _mapper.Map<TaskItem>(request);
        task.CreatedAt = DateTime.UtcNow;
        task.UpdatedAt = DateTime.UtcNow;

        await _repo.AddAsync(task, ct);
        await _repo.SaveChangesAsync(ct);

        return task.TaskID; 
    }

    public async Task<List<TaskDtos.TaskReadDto>> GetAllTasksAsync(CancellationToken ct)
    {
        var tasks = await _repo.GetAllAsync(ct);
        return _mapper.Map<List<TaskDtos.TaskReadDto>>(tasks);
    }

    public async Task<TaskDtos.TaskReadDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        return task is null ? null : _mapper.Map<TaskDtos.TaskReadDto>(task);
    }

    public async Task<bool> UpdateTaskAsync(Guid id, TaskDtos.TaskUpdateDto request, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task is null) return false;

        _mapper.Map(request, task);
        task.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteTaskAsync(Guid id, CancellationToken ct)
    {
        var task = await _repo.GetByIdAsync(id, ct);
        if (task is null) return false;

        // EF трекнет удаление, если удалить вручную:
        var allTasks = await _repo.GetAllAsync(ct);
        allTasks.Remove(task);

        await _repo.SaveChangesAsync(ct);
        return true;
    }
}

*/