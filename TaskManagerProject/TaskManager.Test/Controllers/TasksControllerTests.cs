using AutoMapper;
using Moq;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Controllers;
using TaskManager.Application.DTOs.Activity;

namespace TaskManager.Test.Controllers;

public class TasksControllerTests
{
    private readonly Mock<ITaskService> _serviceMock;
    private readonly Mock<IActivityLogService> _activityLogMock;
    private readonly TasksController _controller;

    public TasksControllerTests()
    {
        _serviceMock = new Mock<ITaskService>();
        _activityLogMock = new Mock<IActivityLogService>();
        _controller = new TasksController(_serviceMock.Object, _activityLogMock.Object);
    }

    [Fact]
    public async Task GetById_Should_Return404_When_TaskNotFound()
    {
        _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskDto?)null);

        var result = await _controller.GetById(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetById_Should_Return200_WithTask_When_Found()
    {
        var task = new TaskDto
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Description = "Desc",
            Status = TaskManager.Domain.Enums.TaskStatus.Todo,
            Priority = TaskPriority.Medium,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Subtasks = [],
            Comments = [],
        };

        _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        var result = await _controller.GetById(Guid.NewGuid(), CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<TaskDto>(okResult.Value);
        Assert.Equal("Test", returned.Title);
    }

    [Fact]
    public async Task Create_Should_Return200_WithId()
    {
        var dto = new CreateTaskRequest
        {
            Title = "New",
            Description = "Desc",
            Priority = TaskPriority.Low,
        };

        var expectedId = Guid.NewGuid();
        _serviceMock.Setup(s => s.CreateAsync(It.IsAny<CreateTaskRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedId);

        var result = await _controller.Create(dto, CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedId, okResult.Value);
    }

    [Fact]
    public async Task Update_Should_Return404_When_TaskNotFound()
    {
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateTaskRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _controller.Update(Guid.NewGuid(), new UpdateTaskRequest(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Update_Should_Return204_When_Success()
    {
        _serviceMock.Setup(s => s.UpdateAsync(It.IsAny<Guid>(), It.IsAny<UpdateTaskRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _controller.Update(Guid.NewGuid(), new UpdateTaskRequest { Title = "Updated" }, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_Should_Return404_When_TaskNotFound()
    {
        _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _controller.Delete(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_Should_Return204_When_Success()
    {
        _serviceMock.Setup(s => s.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _controller.Delete(Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetAll_Should_ReturnPagedResult_When_FilterProvided()
    {
        var paged = new PagedResult<TaskDto>
        {
            Items = new List<TaskDto>
            {
                new() { Id = Guid.NewGuid(), Title = "T1", Description = "", Status = TaskManager.Domain.Enums.TaskStatus.Todo, Priority = TaskPriority.Medium, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Subtasks = [], Comments = [] },
            }.AsReadOnly(),
            TotalCount = 1,
            Page = 1,
            PageSize = 20,
        };

        _serviceMock.Setup(s => s.GetPagedAsync(It.IsAny<TaskFilterRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(paged);

        var result = await _controller.GetAll(page: 1, pageSize: 20, status: TaskManager.Domain.Enums.TaskStatus.Todo, priority: null, userId: null, searchTerm: null, ct: CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsType<PagedResult<TaskDto>>(okResult.Value);
        Assert.Single(returned.Items);
    }

    [Fact]
    public async Task GetAll_Should_ReturnList_When_NoFilter()
    {
        var tasks = new List<TaskDto>
        {
            new() { Id = Guid.NewGuid(), Title = "T1", Description = "", Status = TaskManager.Domain.Enums.TaskStatus.Todo, Priority = TaskPriority.Medium, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Subtasks = [], Comments = [] },
            new() { Id = Guid.NewGuid(), Title = "T2", Description = "", Status = TaskManager.Domain.Enums.TaskStatus.Done, Priority = TaskPriority.High, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow, Subtasks = [], Comments = [] },
        };

        _serviceMock.Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        var result = await _controller.GetAll(page: 1, pageSize: 20, status: null, priority: null, userId: null, searchTerm: null, ct: CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsAssignableFrom<IEnumerable<TaskDto>>(okResult.Value);
        Assert.Equal(2, returned.Count());
    }

    [Fact]
    public async Task Assign_Should_Return404_When_TaskOrUserNotFound()
    {
        _serviceMock.Setup(s => s.AssignAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _controller.Assign(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Assign_Should_Return204_When_Success()
    {
        _serviceMock.Setup(s => s.AssignAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _controller.Assign(Guid.NewGuid(), Guid.NewGuid(), CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetActivityLog_Should_Return200_WithLogs()
    {
        var logs = new List<ActivityLogReadDto>
        {
            new() { Id = Guid.NewGuid(), ActionType = "Created", Timestamp = DateTime.UtcNow },
        };

        _activityLogMock.Setup(s => s.GetByTaskIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(logs);

        var result = await _controller.GetActivityLog(Guid.NewGuid(), CancellationToken.None);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returned = Assert.IsAssignableFrom<IEnumerable<ActivityLogReadDto>>(okResult.Value);
        Assert.Single(returned);
    }
}
