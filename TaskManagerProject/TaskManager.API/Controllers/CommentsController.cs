using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Comments;
using TaskManager.Application.Interfaces;


namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _service;

    public CommentsController(ICommentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCommentRequest dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return Ok(id);
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<IActionResult> GetByTask(Guid taskId, CancellationToken ct)
        => Ok(await _service.GetByTaskIdAsync(taskId, ct));
}

