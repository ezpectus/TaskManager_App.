using System;

namespace TaskManager.Application.DTOs.Attachments;

public class FileAttachmentDto
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public DateTime UploadedAt { get; set; }

    public Guid TaskId { get; set; }

    public Guid? UserId { get; set; }
    public string? Username { get; set; }
}
