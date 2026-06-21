using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastucture.Persistence.EntityConfigurations;

public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
{
    public void Configure(EntityTypeBuilder<ActivityLog> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .HasColumnName("activity_id");

        builder.Property(a => a.ActionType)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(a => a.Timestamp)
            .IsRequired();

        builder.Property(a => a.TaskId)
            .HasColumnName("task_id");

        builder.Property(a => a.UserId)
            .HasColumnName("user_id");

        builder.HasOne(a => a.Task)
            .WithMany(t => t.ActivityLogs)
            .HasForeignKey(a => a.TaskId);

        builder.HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
