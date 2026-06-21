using System;

namespace TaskManager.Application.DTOs.Attachments;

public class CreateAttachmentRequest
{
    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Guid TaskId { get; set; }
    public Guid? UserId { get; set; }
}
