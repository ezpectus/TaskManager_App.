using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Comments;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/comments")]
[ApiVersion("1.0")]
[Authorize]
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
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var comment = await _service.GetByIdAsync(id, ct);
        return comment == null ? NotFound() : Ok(comment);
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<IActionResult> GetByTask(Guid taskId, CancellationToken ct)
        => Ok(await _service.GetByTaskIdAsync(taskId, ct));

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, CreateCommentRequest dto, CancellationToken ct)
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
