using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using TaskManager.Application.DTOs.Comments;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Test.Services;

public class CommentServiceTests
{
    private readonly Mock<ICommentRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IMapper _mapper;
    private readonly CommentService _service;

    public CommentServiceTests()
    {
        _repoMock = new Mock<ICommentRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>(), NullLoggerFactory.Instance).CreateMapper();
        _service = new CommentService(_repoMock.Object, _mapper, _uowMock.Object);
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnGuid_And_SaveChanges()
    {
        var dto = new CreateCommentRequest { Content = "Test comment", TaskId = Guid.NewGuid() };

        var id = await _service.CreateAsync(dto, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        _repoMock.Verify(r => r.AddAsync(It.Is<Comment>(c => c.Content == "Test comment"), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_Should_ReturnFalse_When_CommentNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Comment?)null);

        var result = await _service.UpdateAsync(Guid.NewGuid(), new UpdateCommentRequest(), CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateContent()
    {
        var comment = Comment.Create("Old content", Guid.NewGuid());
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);

        var dto = new UpdateCommentRequest { Content = "New content" };
        var result = await _service.UpdateAsync(Guid.NewGuid(), dto, CancellationToken.None);

        Assert.True(result);
        Assert.Equal("New content", comment.Content);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_Should_SoftDelete()
    {
        var comment = Comment.Create("Test", Guid.NewGuid());
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(comment);

        var result = await _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.True(result);
        Assert.True(comment.IsDeleted);
        Assert.NotNull(comment.DeletedAt);
    }
}
