using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//updated 05.01.26

// Application/Services/ActivityLogService.cs

using AutoMapper;
using TaskManager.Application.DTOs.Activity;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class ActivityLogService : IActivityLogService
{
    private readonly IActivityLogRepository _repo;
    private readonly IMapper _mapper;

    public ActivityLogService(IActivityLogRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ActivityLogReadDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
    {
        var list = await _repo.GetByTaskIdAsync(taskId, ct);
        return _mapper.Map<IEnumerable<ActivityLogReadDto>>(list);
    }

    public async Task<IEnumerable<ActivityLogReadDto>> GetByUserIdAsync(Guid userId, CancellationToken ct)
    {
        var list = await _repo.GetByUserIdAsync(userId, ct);
        return _mapper.Map<IEnumerable<ActivityLogReadDto>>(list);
    }
}

