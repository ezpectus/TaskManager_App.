using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/task-tags")]
[ApiVersion("1.0")]
[Authorize]
public class TaskTagsController : ControllerBase
{
    private readonly ITaskTagService _service;

    public TaskTagsController(ITaskTagService service)
    {
        _service = service;
    }

    [HttpPost("{taskId:guid}/{tagId:guid}")]
    public async Task<IActionResult> Add(Guid taskId, Guid tagId, CancellationToken ct)
    {
        var ok = await _service.AddTagToTaskAsync(taskId, tagId, ct);
        return ok ? NoContent() : Conflict("Tag is already assigned to this task");
    }

    [HttpDelete("{taskId:guid}/{tagId:guid}")]
    public async Task<IActionResult> Remove(Guid taskId, Guid tagId, CancellationToken ct)
    {
        var ok = await _service.RemoveTagFromTaskAsync(taskId, tagId, ct);
        return ok ? NoContent() : NotFound();
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<IActionResult> GetTagsForTask(Guid taskId, CancellationToken ct)
        => Ok(await _service.GetTagsForTaskAsync(taskId, ct));
}
