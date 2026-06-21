using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Enums;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/tasks")]
[ApiVersion("1.0")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskRequest dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var task = await _service.GetByIdAsync(id, ct);
        return task == null ? NotFound() : Ok(task);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] TaskStatus? status = null,
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
}
