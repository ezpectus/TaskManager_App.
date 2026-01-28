using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using System.Data;
using System.Net.Mail;





namespace TaskManager.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // автоматически подтягивает все классы конфигураций Fluent API
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}



// This code defines the ApplicationDbContext class, which inherits from DbContext.
// It represents the database context for the Task Manager application, containing DbSet properties for each entity.
//The OnModelCreating method is overridden to apply configurations from the assembly, allowing for fluent API configurations of the entities.




