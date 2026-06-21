using System;

namespace TaskManager.Application.DTOs.Tags;

public class TagDto
{
    public Guid Id { get; set; }
    public string TagName { get; set; } = null!;
    public string TagColor { get; set; } = "#FFFFFF";
}
