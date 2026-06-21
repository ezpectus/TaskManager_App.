using System;
using System.Collections.Generic;

namespace TaskManager.Domain.Entities;

public class Tag
{
    public Guid Id { get; set; }
    public string TagName { get; set; } = null!;
    public string TagColor { get; set; } = "#FFFFFF";

    public List<TaskTag> TaskTags { get; set; } = new();
}
