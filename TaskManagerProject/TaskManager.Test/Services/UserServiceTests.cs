using AutoMapper;
using Moq;
using TaskManager.Application.DTOs.Users;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Test.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _repoMock;
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly IMapper _mapper;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _repoMock = new Mock<IUserRepository>();
        _uowMock = new Mock<IUnitOfWork>();
        _mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        _service = new UserService(_repoMock.Object, _uowMock.Object, _mapper);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_EmailExists()
    {
        _repoMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var dto = new CreateUserRequest { Username = "test", Email = "test@test.com", Password = "pass" };

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_Should_ReturnGuid_And_HashPassword_When_EmailFree()
    {
        _repoMock.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var dto = new CreateUserRequest { Username = "testuser", Email = "test@test.com", Password = "password123" };

        var id = await _service.CreateAsync(dto, CancellationToken.None);

        Assert.NotEqual(Guid.Empty, id);
        _repoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Username == "testuser" && u.PasswordHash != "password123"), It.IsAny<CancellationToken>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_Should_ReturnFalse_When_UserNotFound()
    {
        _repoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var result = await _service.ValidateCredentialsAsync("test@test.com", "pass", CancellationToken.None);

        Assert.False(result);
    }

    [Fact]
    public async Task ValidateCredentialsAsync_Should_ReturnTrue_When_PasswordMatches()
    {
        var hash = BCrypt.Net.BCrypt.HashPassword("password123");
        _repoMock.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User { Id = Guid.NewGuid(), Username = "test", Email = "test@test.com", PasswordHash = hash });

        var result = await _service.ValidateCredentialsAsync("test@test.com", "password123", CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnFalse_When_UserNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        var result = await _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.False(result);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_Should_ReturnTrue_And_SaveChanges()
    {
        var user = new User { Id = Guid.NewGuid(), Username = "test", Email = "test@test.com" };
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var result = await _service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.True(result);
        _uowMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
