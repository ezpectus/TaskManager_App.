using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence.EntityConfigurations;

public class TaskTagConfiguration : IEntityTypeConfiguration<TaskTag>
{
    public void Configure(EntityTypeBuilder<TaskTag> builder)
    {
        builder.HasKey(tt => new { tt.TaskId, tt.TagId });

        builder.Property(tt => tt.TaskId)
            .HasColumnName("task_id");

        builder.Property(tt => tt.TagId)
            .HasColumnName("tag_id");

        builder.HasQueryFilter(tt => !tt.Task.IsDeleted);

        builder.HasOne(tt => tt.Task)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(tt => tt.TaskId);

        builder.HasOne(tt => tt.Tag)
            .WithMany(t => t.TaskTags)
            .HasForeignKey(tt => tt.TagId);
    }
}
