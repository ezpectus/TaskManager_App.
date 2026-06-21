using System;

namespace TaskManager.Application.DTOs.Comments;

public class CreateCommentRequest
{
    public string Content { get; set; } = null!;
    public Guid TaskId { get; set; }
    public Guid? UserId { get; set; }
}
