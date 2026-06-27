using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence.Contexts;

namespace TaskManager.Test.Integration;

public class UserAndRefreshTokenIntegrationTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
{
    private readonly DatabaseFixture _fixture;

    public UserAndRefreshTokenIntegrationTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    public Task InitializeAsync()
    {
        _fixture.Context.RefreshTokens.RemoveRange(_fixture.Context.RefreshTokens);
        _fixture.Context.Users.RemoveRange(_fixture.Context.Users);
        _fixture.Context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task RefreshToken_Should_Persist_And_Be_Active()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "token_user",
            Email = "token@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.Users.Add(user);
        await _fixture.Context.SaveChangesAsync();

        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = "test-token-123",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.RefreshTokens.Add(token);
        await _fixture.Context.SaveChangesAsync();

        var retrieved = await _fixture.Context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == "test-token-123");

        Assert.NotNull(retrieved);
        Assert.True(retrieved!.IsActive);
        Assert.Null(retrieved.RevokedAt);
    }

    [Fact]
    public async Task RefreshToken_Revoke_Should_Set_RevokedAt()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "revoke_user",
            Email = "revoke@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.Users.Add(user);
        await _fixture.Context.SaveChangesAsync();

        var token = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = "revoke-token-456",
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.RefreshTokens.Add(token);
        await _fixture.Context.SaveChangesAsync();

        token.RevokedAt = DateTime.UtcNow;
        _fixture.Context.RefreshTokens.Update(token);
        await _fixture.Context.SaveChangesAsync();

        var retrieved = await _fixture.Context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == "revoke-token-456");

        Assert.NotNull(retrieved);
        Assert.False(retrieved!.IsActive);
        Assert.NotNull(retrieved.RevokedAt);
    }

    [Fact]
    public async Task Username_UniqueIndex_Should_Prevent_Duplicates()
    {
        var user1 = new User
        {
            Id = Guid.NewGuid(),
            Username = "unique_user",
            Email = "unique1@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.Users.Add(user1);
        await _fixture.Context.SaveChangesAsync();

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            Username = "unique_user",
            Email = "unique2@test.com",
            PasswordHash = "hash",
            CreatedAt = DateTime.UtcNow
        };
        _fixture.Context.Users.Add(user2);

        await Assert.ThrowsAsync<DbUpdateException>(() => _fixture.Context.SaveChangesAsync());
    }
}
