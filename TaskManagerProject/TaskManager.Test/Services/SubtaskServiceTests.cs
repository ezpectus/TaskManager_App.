using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TaskManager.Application.DTOs.Subtasks;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Test.Services;

public class SubtaskServiceTests
{
    private readonly Mock<ISubtaskRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IMapper _mapper;
    private readonly SubtaskService _service;

    public SubtaskServiceTests()
    {
        _repoMock = new Mock<ISubtaskRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance).CreateMapper();
        _service = new SubtaskService(_repoMock.Object, _mapper, _uowMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnGuid_And_SaveChanges()
    {
        var dto = new CreateSubtaskRequest { Title = "Test Subtask", TaskId = Guid.NewGuid() };

        var id = await _service.CreateAsync(dto, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        _repoMock.Verify(r => r.AddAsync(It.Is<Subtask>(s => s.Title == "Test Subtask" && !s.IsCompleted), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnFalse_When_SubtaskNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subtask?)null);

        var result = await _service.UpdateAsync(Guid.NewGuid(), new SubtaskDto(), CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_Rename_And_Complete_When_DtoSaysCompleted()
    {
        var subtask = Subtask.Create("Old Title", Guid.NewGuid());
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subtask);

        var dto = new SubtaskDto { Title = "New Title", IsCompleted = true };
        var result = await _service.UpdateAsync(Guid.NewGuid(), dto, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("New Title", subtask.Title);
        Assert.True(subtask.IsCompleted);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_SoftDelete()
    {
        var subtask = Subtask.Create("Test", Guid.NewGuid());
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(subtask);

        var result = await _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.True(result);
        Assert.True(subtask.IsDeleted);
        Assert.NotNull(subtask.DeletedAt);
    }
}
