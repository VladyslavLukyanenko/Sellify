using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core.Audit
{
  public interface IChangeSetRepository : IRepository<ChangeSet, Guid>
  {
    Task<ChangeSetEntry?> GetPreviousAsync(ChangeSetEntry current, CancellationToken ct = default);
    Task<ChangeSetEntry?> GetEntryByIdAsync(Guid changeSetEntryId, CancellationToken ct = default);
  }
}