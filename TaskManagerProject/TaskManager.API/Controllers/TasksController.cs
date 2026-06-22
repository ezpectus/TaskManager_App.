using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/tasks")]
[ApiVersion("1.0")]
[Authorize]
[EnableRateLimiting("api")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;
    private readonly IActivityLogService _activityLogService;

    public TasksController(ITaskService service, IActivityLogService activityLogService)
    {
        _service = service;
        _activityLogService = activityLogService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id:guid}")]
    [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "id" })]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var task = await _service.GetByIdAsync(id, ct);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpGet]
    [ResponseCache(Duration = 30, VaryByQueryKeys = new[] { "page", "pageSize", "status", "priority", "userId", "searchTerm" })]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] Domain.Enums.TaskStatus? status = null,
        [FromQuery] TaskPriority? priority = null,
        [FromQuery] Guid? userId = null,
        [FromQuery] string? searchTerm = null,
        CancellationToken ct = default)
    {
        if (status.HasValue || priority.HasValue || userId.HasValue ||
            !string.IsNullOrWhiteSpace(searchTerm) || page != 1 || pageSize != 20)
        {
            var filter = new TaskFilterRequest
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                Priority = priority,
                UserId = userId,
                SearchTerm = searchTerm
            };
            return Ok(await _service.GetPagedAsync(filter, ct));
        }

        return Ok(await _service.GetAllAsync(ct));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskRequest dto, CancellationToken ct)
    {
        var ok = await _service.UpdateAsync(id, dto, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpPost("{id:guid}/assign/{userId:guid}")]
    public async Task<IActionResult> Assign(Guid id, Guid userId, CancellationToken ct)
    {
        var ok = await _service.AssignAsync(id, userId, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("{id:guid}/activity")]
    public async Task<IActionResult> GetActivityLog(Guid id, CancellationToken ct)
    {
        var logs = await _activityLogService.GetByTaskIdAsync(id, ct);
        return Ok(logs);
    }
}
