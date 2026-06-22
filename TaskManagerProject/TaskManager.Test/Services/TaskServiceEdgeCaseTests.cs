using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Test.Services;

public class TaskServiceEdgeCaseTests
{
    private readonly Mock<ITaskRepository> _repoMock;
    private readonly Mock<IUserRepository> _userRepoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IMapper _mapper;
    private readonly TaskService _service;

    public TaskServiceEdgeCaseTests()
    {
        _repoMock = new Mock<ITaskRepository>();
        _userRepoMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance).CreateMapper();
        _service = new TaskService(_repoMock.Object, _userRepoMock.Object, _mapper, _uowMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnEmptyList_When_NoTasks()
    {
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var result = await _service.GetAllAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnMappedDtos()
    {
        var tasks = new List<TaskItem>
        {
            TaskItem.Create("Task 1", "Desc 1", TaskPriority.Low),
            TaskItem.Create("Task 2", "Desc 2", TaskPriority.High),
        };

        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        var result = await _service.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, result.Count());
        Assert.Contains(result, t => t.Title == "Task 1");
        Assert.Contains(result, t => t.Title == "Task 2");
    }

    [Fact]
    public async Task GetPagedAsync_Should_ReturnCorrectPagination()
    {
        var tasks = new List<TaskItem>
        {
            TaskItem.Create("Task 1", "Desc", TaskPriority.Medium),
            TaskItem.Create("Task 2", "Desc", TaskPriority.Medium),
        };

        _repoMock.Setup(r => r.GetPagedAsync(
                It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<TaskManager.Domain.Enums.TaskStatus?>(), It.IsAny<TaskPriority?>(),
                It.IsAny<Guid?>(), It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((tasks.AsReadOnly(), 10));

        var filter = new TaskFilterRequest { Page = 2, PageSize = 2 };
        var result = await _service.GetPagedAsync(filter, CancellationToken.None);

        Assert.Equal(10, result.TotalCount);
        Assert.Equal(2, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(5, result.TotalPages);
    }

    [Fact]
    public async Task GetPagedAsync_Should_ReturnEmpty_When_NoTasks()
    {
        _repoMock.Setup(r => r.GetPagedAsync(
                It.IsAny<int>(), It.IsAny<int>(),
                It.IsAny<TaskManager.Domain.Enums.TaskStatus?>(), It.IsAny<TaskPriority?>(),
                It.IsAny<Guid?>(), It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Array.Empty<TaskItem>().AsReadOnly(), 0));

        var filter = new TaskFilterRequest { Page = 1, PageSize = 10 };
        var result = await _service.GetPagedAsync(filter, CancellationToken.None);

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateStatus_When_OnlyStatusProvided()
    {
        var task = TaskItem.Create("Test", "Desc", TaskPriority.Medium);
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        var dto = new UpdateTaskRequest { Status = TaskManager.Domain.Enums.TaskStatus.InProgress };
        var result = await _service.UpdateAsync(Guid.NewGuid(), dto, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(TaskManager.Domain.Enums.TaskStatus.InProgress, task.Status);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdatePriority_When_OnlyPriorityProvided()
    {
        var task = TaskItem.Create("Test", "Desc", TaskPriority.Medium);
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        var dto = new UpdateTaskRequest { Priority = TaskPriority.High };
        var result = await _service.UpdateAsync(Guid.NewGuid(), dto, CancellationToken.None);

        Assert.True(result);
        Assert.Equal(TaskPriority.High, task.Priority);
    }

    [Fact]
    public async Task CreateAsync_Should_SetCreatedAtAndUpdatedAt()
    {
        var dto = new CreateTaskRequest
        {
            Title = "New Task",
            Description = "New Desc",
            Priority = TaskPriority.Low,
        };

        TaskItem? captured = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
            .Callback<TaskItem, CancellationToken>((t, _) => captured = t)
            .Returns(Task.CompletedTask);

        await _service.CreateAsync(dto, CancellationToken.None);

        Assert.NotNull(captured);
        Assert.True(captured!.CreatedAt <= DateTime.UtcNow);
        Assert.True(captured.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public async Task DeleteAsync_Should_SetDeletedAt_When_TaskExists()
    {
        var task = TaskItem.Create("Test", "Desc", TaskPriority.Medium);
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        await _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.True(task.IsDeleted);
        Assert.NotNull(task.DeletedAt);
        Assert.True(task.DeletedAt <= DateTime.UtcNow);
    }
}
