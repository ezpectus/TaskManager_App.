using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Persistence.Contexts;
using TaskManager.Infrastructure.Persistence.Interceptors;
using Testcontainers.PostgreSql;

namespace TaskManager.Test.Integration;

public class DatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer? _container;
    private bool _dockerAvailable;

    public ApplicationDbContext Context { get; private set; } = null!;
    public bool IsAvailable => _dockerAvailable;

    public async Task InitializeAsync()
    {
        try
        {
            _container = new PostgreSqlBuilder()
                .WithImage("postgres:16-alpine")
                .WithDatabase("TaskManagerTestDb")
                .WithUsername("test")
                .WithPassword("test")
                .Build();

            await _container.StartAsync();
            _dockerAvailable = true;

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(_container.GetConnectionString())
                .Options;

            Context = new ApplicationDbContext(options, new ActivityLogInterceptor());
            await Context.Database.EnsureCreatedAsync();
        }
        catch
        {
            _dockerAvailable = false;
        }
    }

    public async Task DisposeAsync()
    {
        if (Context != null)
            await Context.DisposeAsync();
        if (_container != null)
            await _container.DisposeAsync();
    }
}
