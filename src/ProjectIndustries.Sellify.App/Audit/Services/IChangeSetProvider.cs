using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Audit.Data;
using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.Audit.Services
{
  public interface IChangeSetProvider
  {
    Task<IPagedList<ChangeSetData>> GetChangeSetPageAsync(ChangeSetPageRequest pageRequest,
      CancellationToken ct = default);

    Task<ChangesetEntryPayloadData?> GetPayloadAsync(Guid changeSetEntryId, CancellationToken ct = default);
  }
}