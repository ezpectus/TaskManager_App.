using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Mapping;
using TaskManager.Application.Services;
using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Persistence.Repositories;
using TaskManager.Infrastucture.Persistence.Contexts;
using TaskManager.Infrastucture.Persistence.Repositories;




// Updated version with services registration, 27.01.26


namespace TaskManager.Infrastucture.DepenInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
     this IServiceCollection services,
     IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });


        services.AddRepositories();
        services.AddServices();

        return services;
    }


    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ITaskRepository, TaskRepository>()
            .AddScoped<ISubtaskRepository, SubtaskRepository>()
            .AddScoped<ICommentRepository, CommentRepository>()
            .AddScoped<ITagRepository, TagRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IFileAttachmentRepository, FileAttachmentRepository>()
            .AddScoped<IActivityLogRepository, ActivityLogRepository>()
            .AddScoped<ITaskTagRepository, TaskTagRepository>()
            .AddScoped<IUserRoleRepository, UserRoleRepository>();

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
            .AddScoped<IRoleService, RoleService>();
}
