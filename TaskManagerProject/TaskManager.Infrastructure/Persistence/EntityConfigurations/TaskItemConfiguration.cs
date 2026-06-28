using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.EntityConfigurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("task_id");

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.Priority)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(t => t.Deadline).IsRequired();
        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();

        builder.Property(t => t.UserId)
            .HasColumnName("user_id");

        builder.HasQueryFilter(t => !t.IsDeleted);

        builder.Property(t => t.RowVersion)
            .IsConcurrencyToken()
            .HasDefaultValueSql("xmin()");

        builder.HasIndex(t => t.Deadline);
        builder.HasIndex(t => t.Status);
        builder.HasIndex(t => t.Priority);
        builder.HasIndex(t => t.UserId);
        builder.HasIndex(t => new { t.Status, t.Priority });

        builder.HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserId);

        builder.HasMany(t => t.Subtasks)
            .WithOne(s => s.Task)
            .HasForeignKey(s => s.TaskId);

        builder.HasMany(t => t.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId);

        builder.HasMany(t => t.Attachments)
            .WithOne(a => a.Task)
            .HasForeignKey(a => a.TaskId);

        builder.HasMany(t => t.TaskTags)
            .WithOne(tt => tt.Task)
            .HasForeignKey(tt => tt.TaskId);

        builder.HasMany(t => t.ActivityLogs)
            .WithOne(al => al.Task)
            .HasForeignKey(al => al.TaskId);
    }
}