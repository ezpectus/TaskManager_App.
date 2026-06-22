using System.Collections.Generic;
using System.Linq;
using TaskManager.Application.DTOs.Tasks;

namespace TaskManager.Application.Sorting;

public static class TaskSorter
{
    public static IEnumerable<TaskDto> SortByPriority(IEnumerable<TaskDto> tasks)
    {
        return tasks.OrderByDescending(t => t.Priority);
    }
}
