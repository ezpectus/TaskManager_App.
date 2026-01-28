using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
// 28.01.26 v 1.01

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/task-tags")]
public class TaskTagsController : ControllerBase
{
    private readonly ITaskTagService _service;

    public TaskTagsController(ITaskTagService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid taskId, Guid tagId, CancellationToken ct)
    {
        await _service.AddTagToTaskAsync(taskId, tagId, ct);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(Guid taskId, Guid tagId, CancellationToken ct)
    {
        await _service.RemoveTagFromTaskAsync(taskId, tagId, ct);
        return NoContent();
    }
}
