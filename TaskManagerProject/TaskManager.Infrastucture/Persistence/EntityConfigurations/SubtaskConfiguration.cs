using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastucture.Persistence.EntityConfigurations;

public class SubtaskConfiguration : IEntityTypeConfiguration<Subtask>
{
    public void Configure(EntityTypeBuilder<Subtask> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("subtask_id");

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.IsCompleted)
            .IsRequired();

        builder.Property(s => s.TaskId)
            .HasColumnName("task_id");

        builder.HasQueryFilter(s => !s.IsDeleted);

        builder.HasOne(s => s.Task)
            .WithMany(t => t.Subtasks)
            .HasForeignKey(s => s.TaskId);
    }
}
