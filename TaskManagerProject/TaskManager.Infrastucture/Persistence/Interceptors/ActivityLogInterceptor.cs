using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastucture.Persistence.Interceptors;

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
                Timestamp = DateTime.UtcNow,
                ActionType = entry.State switch
                {
                    EntityState.Added => "Created",
                    EntityState.Modified => "Updated",
                    EntityState.Deleted => "Deleted",
                    _ => "Unknown"
                }
            };

            context.ActivityLogs.Add(log);
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
