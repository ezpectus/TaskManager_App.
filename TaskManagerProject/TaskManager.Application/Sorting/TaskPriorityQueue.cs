using System.Collections.Generic;
using TaskManager.Application.DTOs.Tasks;

namespace TaskManager.Application.Sorting;

public class TaskPriorityQueue
{
    private readonly PriorityQueue<TaskDto, int> _queue = new();

    public void Add(TaskDto task)
    {
        _queue.Enqueue(task, -(int)task.Priority);
    }

    public TaskDto? Pop()
    {
        if (_queue.Count == 0)
            return null;

        return _queue.Dequeue();
    }

    public bool IsEmpty => _queue.Count == 0;
}