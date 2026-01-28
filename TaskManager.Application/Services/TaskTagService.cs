using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
//updated 26.01.26

// Application/Services/TaskTagService.cs
namespace TaskManager.Application.Services;

public class TaskTagService : ITaskTagService
{
    private readonly ITaskTagRepository _repo;

    public TaskTagService(ITaskTagRepository repo)
        => _repo = repo;

    public async Task<bool> AddTagToTaskAsync(Guid taskId, Guid tagId, CancellationToken ct)
    {
        if (await _repo.ExistsAsync(taskId, tagId, ct))
            return false;

        await _repo.AddAsync(new TaskTag
        {
            TaskId = taskId,
            TagId = tagId
        }, ct);

        return true;
    }

    public async Task<bool> RemoveTagFromTaskAsync(Guid taskId, Guid tagId, CancellationToken ct)
        => await _repo.RemoveAsync(taskId, tagId, ct);

    public async Task<IEnumerable<Guid>> GetTagsForTaskAsync(Guid taskId, CancellationToken ct)
    {
        var tags = await _repo.GetTagsByTaskIdAsync(taskId, ct);
        return tags.Select(t => t.Id);
    }


}
