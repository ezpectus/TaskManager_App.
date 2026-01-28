using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Tasks;

//updated 26/01/26

// Application/Interfaces/ITaskService.cs

namespace TaskManager.Application.Interfaces;

public interface ITaskService
{
    Task<Guid> CreateAsync(CreateTaskRequest dto, CancellationToken ct);
    Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<TaskDto>> GetAllAsync(CancellationToken ct);
    Task<bool> UpdateAsync(Guid id, UpdateTaskRequest dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);

}


/*
namespace TaskManager.Application.Interfaces;

    public interface ITaskService
    {
        Task<Guid> CreateTaskAsync(TaskDtos.TaskCreateDto request, CancellationToken ct);
        Task<List<TaskDtos.TaskReadDto>> GetAllTasksAsync(CancellationToken ct);
        Task<TaskDtos.TaskReadDto?> GetByIdAsync(Guid id, CancellationToken ct);
        Task<bool> UpdateTaskAsync(Guid id, TaskDtos.TaskUpdateDto request, CancellationToken ct);
        Task<bool> DeleteTaskAsync(Guid id, CancellationToken ct);

        // метод сортировки
        Task<List<TaskDtos.TaskReadDto>> GetTasksSortedAsync(string sortBy, CancellationToken ct);
    }


 
*/