using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Application.DTOs.Attachments;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Application.Services;

public class FileAttachmentService : IFileAttachmentService
{
    private readonly IFileAttachmentRepository _repo;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FileAttachmentService(IFileAttachmentRepository repo, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repo = repo;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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
        await _unitOfWork.SaveChangesAsync(ct);
        return entity.Id;
    }


    public async Task<FileAttachmentDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var a = await _repo.GetByIdAsync(id, ct);
        return a == null ? null : _mapper.Map<FileAttachmentDto>(a);
    }

    public async Task<IEnumerable<FileAttachmentDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
    {
        var list = await _repo.GetByTaskIdAsync(taskId, ct);
        return _mapper.Map<IEnumerable<FileAttachmentDto>>(list);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var a = await _repo.GetByIdAsync(id, ct);
        if (a == null) return false;

        await _repo.DeleteAsync(a, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
