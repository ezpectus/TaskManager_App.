using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 26/01/26
namespace TaskManager.Application.DTOs.Tasks;

using TaskManager.Application.DTOs.Comments;
using TaskManager.Application.DTOs.Subtasks;
using TaskManager.Application.DTOs.Attachments;
using TaskManager.Application.DTOs.Tags;
using TaskManager.Application.DTOs.Activity;

public class TaskDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Priority { get; set; } = null!;

    public DateTime Deadline { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Guid? UserId { get; set; }
    public string? Username { get; set; }

    public List<SubtaskDto> Subtasks { get; set; } = new();
    public List<CommentDto> Comments { get; set; } = new();
    public List<FileAttachmentDto> Attachments { get; set; } = new();
    public List<TagDto> Tags { get; set; } = new();
    public List<ActivityLogReadDto> ActivityLogs { get; set; } = new();
}
