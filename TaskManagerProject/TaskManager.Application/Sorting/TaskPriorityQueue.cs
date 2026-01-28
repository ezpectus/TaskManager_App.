using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using System.Collections.Generic;
using static global::TaskManager.Application.DTOs.TaskDtos;



namespace TaskManager.Application.Sorting;


// Очередь задач по приоритету (Critical выходит первым)
public class TaskPriorityQueue
{
    private readonly PriorityQueue<TaskReadDto, int> _queue = new();

    // Добавить задачу в очередь (инвертируем приоритет, чтобы критичные шли первыми)
    public void Add(TaskReadDto task)
    {
        _queue.Enqueue(task, -(int)task.Priority);
    }

    // Забрать задачу с наивысшим приоритетом
    public TaskReadDto? Pop()
    {
        if (_queue.Count == 0)
            return null;

        return _queue.Dequeue();
    }

    // Проверка, пустая ли очередь
    public bool IsEmpty => _queue.Count == 0;
}



// This class implements a priority queue for tasks, where tasks with higher priority (lower integer value) are dequeued first.