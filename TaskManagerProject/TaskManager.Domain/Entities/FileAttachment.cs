using System;

namespace TaskManager.Domain.Entities;

public class FileAttachment
{
    public Guid Id { get; set; }

    public string FileName { get; set; } = null!;
    public string Url { get; set; } = null!;
    public DateTime UploadedAt { get; set; }

    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;

    public Guid? UserId { get; set; }
    public User? User { get; set; }
}
