using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence.Contexts;

namespace TaskManager.Infrastructure.Persistence.Interceptors;

public class ActivityLogInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context == null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        var taskEntries = context.ChangeTracker.Entries<TaskItem>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        foreach (var entry in taskEntries)
        {
            var log = new ActivityLog
            {
                Id = Guid.NewGuid(),
                TaskId = entry.Entity.Id,
                UserId = entry.Entity.UserId,
                Timestamp = DateTime.UtcNow,
                ActionType = entry.State switch
                {
                    EntityState.Added => "Created",
                    EntityState.Modified => "Updated",
                    EntityState.Deleted => "Deleted",
                    _ => "Unknown"
                }
            };

            if (context is ApplicationDbContext appContext)
            {
                appContext.ActivityLogs.Add(log);
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
