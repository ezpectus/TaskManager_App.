using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Attachments;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/attachments")]
[ApiVersion("1.0")]
[Authorize]
public class FileAttachmentsController : ControllerBase
{
    private readonly IFileAttachmentService _service;

    public FileAttachmentsController(IFileAttachmentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Upload(CreateAttachmentRequest dto, CancellationToken ct)
    {
        var id = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var attachment = await _service.GetByIdAsync(id, ct);
        return attachment == null ? NotFound() : Ok(attachment);
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<IActionResult> GetByTask(Guid taskId, CancellationToken ct)
        => Ok(await _service.GetByTaskIdAsync(taskId, ct));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var ok = await _service.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
