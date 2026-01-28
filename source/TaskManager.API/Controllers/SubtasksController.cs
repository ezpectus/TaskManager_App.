using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Subtasks;
using TaskManager.Application.Interfaces;
// 28.01.26 v 1.01


namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/subtasks")]
public class SubtasksController : ControllerBase
{
    private readonly ISubtaskService _service;

    public SubtasksController(ISubtaskService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSubtaskRequest dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return Ok(id);
    }

    [HttpGet("task/{taskId:guid}")]
   // public async Task<IActionResult> GetByTask(Guid taskId, CancellationToken ct)
      //  => Ok(await _service.GetByTaskIdAsync(taskId, ct));

    public async Task<IActionResult> GetByTask(Guid taskId, CancellationToken ct)
      => Ok(await _service.GetByIdAsync(taskId, ct));
}
