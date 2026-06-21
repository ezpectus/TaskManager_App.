using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class TaskTagService : ITaskTagService
{
    private readonly ITaskTagRepository _repo;
    private readonly IUnitOfWork _unitOfWork;

    public TaskTagService(ITaskTagRepository repo, IUnitOfWork unitOfWork)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> AddTagToTaskAsync(Guid taskId, Guid tagId, CancellationToken ct)
    {
        if (await _repo.ExistsAsync(taskId, tagId, ct))
            return false;

        await _repo.AddAsync(new TaskTag
        {
            TaskId = taskId,
            TagId = tagId
        }, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return true;
    }

    public async Task<bool> RemoveTagFromTaskAsync(Guid taskId, Guid tagId, CancellationToken ct)
    {
        var removed = await _repo.RemoveAsync(taskId, tagId, ct);
        if (removed)
            await _unitOfWork.SaveChangesAsync(ct);
        return removed;
    }

    public async Task<IEnumerable<Guid>> GetTagsForTaskAsync(Guid taskId, CancellationToken ct)
    {
        var tags = await _repo.GetTagsByTaskIdAsync(taskId, ct);
        return tags.Select(t => t.Id);
    }


}
