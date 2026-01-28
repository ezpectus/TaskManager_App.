using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26


namespace TaskManager.Application.DTOs.Subtasks;

public class CreateSubtaskRequest
{
    public string Title { get; set; } = null!;
    public Guid TaskId { get; set; }
}
