using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectIndustries.Sellify.Core;
using ProjectIndustries.Sellify.Core.Audit;
using ProjectIndustries.Sellify.Core.Audit.Services;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Infra.Audit
{
  public class DbContextChangesAuditor : IDbContextChangesAuditor
  {
    private readonly ICurrentChangeSetProvider _changeSetProvider;
    private readonly IChangeSetMapperProvider _changeSetMapperProvider;

    public DbContextChangesAuditor(ICurrentChangeSetProvider changeSetProvider,
      IChangeSetMapperProvider changeSetMapperProvider)
    {
      _changeSetProvider = changeSetProvider;
      _changeSetMapperProvider = changeSetMapperProvider;
    }

    public void AuditChanges(DbContext ctx)
    {
      ChangeSet? changeSet = _changeSetProvider.CurrentChangSet;
      if (changeSet == null)
      {
        return;
      }

      var entries = ctx.ChangeTracker
        .Entries()
        .Where(_ =>
          _changeSetMapperProvider.HasMapperForType(_.Metadata.ClrType)
          && _.Metadata.ClrType.IsEntity()
          && _.State != EntityState.Detached && _.State != EntityState.Unchanged)
        .ToArray();

      if (!entries.Any())
      {
        return;
      }

      foreach (EntityEntry entry in entries)
      {
        var mapper = _changeSetMapperProvider.GetMapper(entry.Metadata.ClrType);

        ChangeType changeType = entry.State switch
        {
          EntityState.Modified => entry.Entity is ISoftRemovable sr && sr.IsRemoved()
            ? ChangeType.Removal
            : ChangeType.Modification,
          EntityState.Added => ChangeType.Creation,
          EntityState.Deleted => ChangeType.Removal,
          _ => throw new IndexOutOfRangeException("Not supported state change tracking: " + entry.State)
        };

        ChangeSetEntry changeSetEntry = mapper.Map(entry.Entity, changeType);
        if (changeSetEntry != null)
        {
          changeSet.AddEntry(changeSetEntry);
        }
      }
    }
  }
}