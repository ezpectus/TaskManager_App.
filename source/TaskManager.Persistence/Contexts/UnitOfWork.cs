using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Persistence.Contexts;
using TaskManager.Persistence.Repositories;





namespace TaskManager.Persistence.Contexts;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly IServiceProvider _provider;

    public UnitOfWork(ApplicationDbContext context, IServiceProvider provider)
    {
        _context = context;
        _provider = provider;
    }

    public TRepository GetRepository<TRepository>() where TRepository : class
        => _provider.GetRequiredService<TRepository>();

    public int Complete() => _context.SaveChanges();

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}

/*
namespace TaskManager.Persistence.Contexts

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }
        public ITaskRepository Tasks { get; }
        public ISubtaskRepository Subtasks { get; }
        public ICommentRepository Comments { get; }
        public ITagRepository Tags { get; }
        public IRoleRepository Roles { get; }
        public IAttachmentRepository Attachments { get; }
        public IActivityLogRepository ActivityLogs { get; }
        public ITaskTagRepository TaskTags { get; }
        public IUserRoleRepository UserRoles { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository users,
            ITaskRepository tasks,
            ISubtaskRepository subtasks,
            ICommentRepository comments,
            ITagRepository tags,
            IRoleRepository roles,
            IAttachmentRepository attachments,
            IActivityLogRepository activityLogs,
            ITaskTagRepository taskTags,
            IUserRoleRepository userRoles)
        {
            _context = context;
            Users = users;
            Tasks = tasks;
            Subtasks = subtasks;
            Comments = comments;
            Tags = tags;
            Roles = roles;
            Attachments = attachments;
            ActivityLogs = activityLogs;
            TaskTags = taskTags;
            UserRoles = userRoles;
        }

        public int Complete() => _context.SaveChanges();

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);

        public void Dispose() => _context.Dispose();
    }
}

  
*/