using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Audit;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Infra.Repositories;

namespace ProjectIndustries.Sellify.Infra.Audit
{
  public class EfChangeSetRepository
    : EfCrudRepository<ChangeSet, Guid>, IChangeSetRepository
  {
    public EfChangeSetRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    private IQueryable<ChangeSetEntry> ChangeSetEntries => DataSource.AsNoTracking().SelectMany(_ => _.Entries);

    public Task<ChangeSetEntry?> GetPreviousAsync(ChangeSetEntry current, CancellationToken ct = default)
    {
      return ChangeSetEntries
        .Where(_ => _.EntityId == current.EntityId && _.EntityType == current.EntityType &&
                    _.CreatedAt < current.CreatedAt)
        .OrderByDescending(_ => _.CreatedAt)
        .FirstOrDefaultAsync(ct)!;
    }

    public Task<ChangeSetEntry?> GetEntryByIdAsync(Guid changeSetEntryId, CancellationToken ct = default)
    {
      return ChangeSetEntries.FirstOrDefaultAsync(_ => _.Id == changeSetEntryId, ct)!;
    }
  }
}