using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26


namespace TaskManager.Application.DTOs.Subtasks;

public class SubtaskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public bool IsCompleted { get; set; }

    public Guid TaskId { get; set; }
}
