using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastucture.Persistence.Interceptors;

namespace TaskManager.Infrastucture.Persistence.Contexts;

public class ApplicationDbContext : DbContext
{
    private readonly ActivityLogInterceptor _activityLogInterceptor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ActivityLogInterceptor activityLogInterceptor)
        : base(options)
    {
        _activityLogInterceptor = activityLogInterceptor;
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Subtask> Subtasks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<FileAttachment> Attachments { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<TaskTag> TaskTags { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_activityLogInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}




