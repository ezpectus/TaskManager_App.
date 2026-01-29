using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using AutoMapper;
using TaskManager.Application.DTOs.Attachments;
//updated 05.01.26

// Application/Services/FileAttachmentService.cs
namespace TaskManager.Application.Services;


public class FileAttachmentService : IFileAttachmentService
{
    private readonly IFileAttachmentRepository _repo;

    public FileAttachmentService(IFileAttachmentRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> CreateAsync(CreateAttachmentRequest dto, CancellationToken ct)
    {
        var entity = new FileAttachment
        {
            Id = Guid.NewGuid(),
            FileName = dto.FileName,
            Url = dto.Url,
            UploadedAt = DateTime.UtcNow,
            TaskId = dto.TaskId,
            UserId = dto.UserId
        };

        await _repo.AddAsync(entity, ct);
        return entity.Id;
    }


    public async Task<FileAttachmentDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var a = await _repo.GetByIdAsync(id, ct);
        if (a == null) return null;

        return new FileAttachmentDto
        {
            Id = a.Id,
            FileName = a.FileName,
            Url = a.Url,
            UploadedAt = a.UploadedAt,
            TaskId = a.TaskId,
            UserId = a.UserId,
            Username = a.User?.Username
        };
    }

    public async Task<IEnumerable<FileAttachmentDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
    {
        var list = await _repo.GetByTaskIdAsync(taskId, ct);

        return list.Select(a => new FileAttachmentDto
        {
            Id = a.Id,
            FileName = a.FileName,
            Url = a.Url,
            UploadedAt = a.UploadedAt,
            TaskId = a.TaskId,
            UserId = a.UserId,
            Username = a.User?.Username
        });
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var a = await _repo.GetByIdAsync(id, ct);
        if (a == null) return false;

        await _repo.DeleteAsync(a, ct);
        return true;
    }
}
