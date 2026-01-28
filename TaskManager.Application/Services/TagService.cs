using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using TaskManager.Application.DTOs.Tags;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Application.Interfaces;
//updated 05.01.26


// Application/Services/TagService.cs
namespace TaskManager.Application.Services;

public class TagService : ITagService
{
    private readonly ITagRepository _repo;
    private readonly IMapper _mapper;

    public TagService(ITagRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<Guid> CreateAsync(TagDto dto, CancellationToken ct)
    {
        var tag = _mapper.Map<Tag>(dto);
        tag.Id = Guid.NewGuid();

        await _repo.AddAsync(tag, ct);
        return tag.Id;
    }

    public async Task<TagDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var tag = await _repo.GetByIdAsync(id, ct);
        return tag == null ? null : _mapper.Map<TagDto>(tag);
    }

    public async Task<IEnumerable<TagDto>> GetAllAsync(CancellationToken ct)
    {
        var list = await _repo.GetAllAsync(ct);
        return _mapper.Map<IEnumerable<TagDto>>(list);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var tag = await _repo.GetByIdAsync(id, ct);
        if (tag == null) return false;

        await _repo.DeleteAsync(tag, ct);
        return true;
    }
}
