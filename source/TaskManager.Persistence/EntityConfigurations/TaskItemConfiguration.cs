using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;



namespace TaskManager.Persistence.EntityConfigurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(t => t.TaskId);

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(2000);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Priority)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Deadline).IsRequired();
            builder.Property(t => t.CreatedAt).IsRequired();
            builder.Property(t => t.UpdatedAt).IsRequired();

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
}
