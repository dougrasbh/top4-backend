using BryophytaClassifier.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BryophytaClassifier.Database;

public class AuditableEntitiesInterceptor : SaveChangesInterceptor {
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result) {
        ArgumentNullException.ThrowIfNull(eventData);

        BeforeSaveTriggers(eventData.Context);
        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default) {
        ArgumentNullException.ThrowIfNull(eventData);

        BeforeSaveTriggers(eventData.Context);
        return ValueTask.FromResult(result);
    }

    private static void BeforeSaveTriggers(DbContext? context) {
        var entries = context?.ChangeTracker.Entries();
        if (entries == null) return;

        foreach (var entityEntry in entries) {
            if (entityEntry.Entity is not IAuditable entity ||
                entityEntry.State != EntityState.Added && entityEntry.State != EntityState.Modified) {
                continue;
            }
            entity.LastUpdateTime = DateTimeOffset.UtcNow;

            if (entityEntry.State == EntityState.Added) {
                entity.CreationTime = DateTimeOffset.UtcNow;
            }
        }
    }
}