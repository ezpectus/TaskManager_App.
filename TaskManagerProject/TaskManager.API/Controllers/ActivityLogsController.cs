using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/activity")]
[ApiVersion("1.0")]
[Authorize]
public class ActivityLogsController : ControllerBase
{
    private readonly IActivityLogService _service;

    public ActivityLogsController(IActivityLogService service)
    {
        _service = service;
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<IActionResult> GetByTask(Guid taskId, CancellationToken ct)
        => Ok(await _service.GetByTaskIdAsync(taskId, ct));

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId, CancellationToken ct)
        => Ok(await _service.GetByUserIdAsync(userId, ct));
}
