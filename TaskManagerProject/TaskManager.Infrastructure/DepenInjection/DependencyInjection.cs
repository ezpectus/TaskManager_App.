using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence.Contexts;
using TaskManager.Infrastructure.Persistence.Interceptors;
using TaskManager.Infrastructure.Persistence.Repositories;

namespace TaskManager.Infrastructure.DepenInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<ActivityLogInterceptor>();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });

        services.AddValidatorsFromAssembly(typeof(TaskService).Assembly);

        services.AddRepositories();
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITaskRepository, TaskRepository>()
            .AddScoped<ISubtaskRepository, SubtaskRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()
            .AddScoped<ITagRepository, TagRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IFileAttachmentRepository, FileAttachmentRepository>()
            .AddScoped<IActivityLogRepository, ActivityLogRepository>()
            .AddScoped<ITaskTagRepository, TaskTagRepository>()
            .AddScoped<IUserRoleRepository, UserRoleRepository>()
            .AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IUserService, UserService>()
            .AddScoped<ITaskService, TaskService>()
            .AddScoped<ISubtaskService, SubtaskService>()
            .AddScoped<ICommentService, CommentService>()
            .AddScoped<ITagService, TagService>()
            .AddScoped<ITaskTagService, TaskTagService>()
            .AddScoped<IActivityLogService, ActivityLogService>()
            .AddScoped<IFileAttachmentService, FileAttachmentService>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<IUserRoleService, UserRoleService>()
            .AddScoped<IAuthService, AuthService>();
}
