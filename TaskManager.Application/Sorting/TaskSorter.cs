using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;



using static TaskManager.Application.DTOs.TaskDtos;

namespace TaskManager.Application.Sorting
{
    // Простая сортировка задач по приоритету
    public static class TaskSorter
    {
        // Сортирует задачи от самых важных (Critical) до обычных (Low)
        public static IEnumerable<TaskReadDto> SortByPriority(IEnumerable<TaskReadDto> tasks)
        {
            return tasks.OrderByDescending(t => t.Priority);
        }
    }
}

