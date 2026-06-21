using AutoMapper;
using Moq;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Test.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IMapper _mapper;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _repoMock = new Mock<ITaskRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _service = new TaskService(_repoMock.Object, _mapper, _uowMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnGuid_And_CallSaveChanges()
    {
        var dto = new CreateTaskRequest
        {
            Title = "Test Task",
            Description = "Test Description",
            Priority = TaskPriority.High
        };

        var id = await _service.CreateAsync(dto, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnNull_When_TaskNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        var result = await _service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_Should_ReturnDto_When_TaskFound()
    {
        var task = TaskItem.Create("Test", "Desc", TaskPriority.Medium);
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        var result = await _service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("Test", result!.Title);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnFalse_When_TaskNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        var result = await _service.UpdateAsync(Guid.NewGuid(), new UpdateTaskRequest(), CancellationToken.None);

        Assert.False(result);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnTrue_And_SaveChanges_When_TaskFound()
    {
        var task = TaskItem.Create("Test", "Desc", TaskPriority.Medium);
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        var dto = new UpdateTaskRequest { Title = "Updated", Description = "Updated Desc" };
        var result = await _service.UpdateAsync(Guid.NewGuid(), dto, CancellationToken.None);

        Assert.True(result);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnFalse_When_TaskNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem?)null);

        var result = await _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(result);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Should_SoftDelete_And_SaveChanges()
    {
        var task = TaskItem.Create("Test", "Desc", TaskPriority.Medium);
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        var result = await _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.True(result);
        Assert.True(task.IsDeleted);
        Assert.NotNull(task.DeletedAt);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
