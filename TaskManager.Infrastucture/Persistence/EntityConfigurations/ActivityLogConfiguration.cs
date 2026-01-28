using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
//updated version 27.01.26


namespace TaskManager.Infrastucture.Persistence.EntityConfigurations;
// new version
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



//old version

/*

    public class ActivityLogConfiguration : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.HasKey(a => a.ActivityId);

            builder.Property(a => a.ActionType)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(a => a.Timestamp)
                .IsRequired();

            builder.HasOne(a => a.Task)
                .WithMany(t => t.ActivityLogs)
                .HasForeignKey(a => a.TaskId);

            builder.HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
   
*/
