using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs;
using TaskManager.Application.DTOs.Attachments;
using TaskManager.Application.Interfaces;
// 28.01.26 v 1.01


namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/attachments")]
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
        return Ok(id);
    }
}
