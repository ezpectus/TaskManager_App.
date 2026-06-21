using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastucture.Persistence.EntityConfigurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("comment_id");

        builder.Property(c => c.Content)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.TaskId)
            .HasColumnName("task_id");

        builder.Property(c => c.UserId)
            .HasColumnName("user_id");

        builder.HasQueryFilter(c => !c.IsDeleted);

        builder.HasOne(c => c.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId);

        builder.HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
