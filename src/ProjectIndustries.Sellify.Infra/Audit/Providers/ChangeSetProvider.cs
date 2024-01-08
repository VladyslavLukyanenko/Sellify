using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.App.Audit.Data;
using ProjectIndustries.Sellify.App.Audit.Services;
using ProjectIndustries.Sellify.Core.Audit;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.Infra.Converters;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.Infra.Audit.Providers
{
  public class ChangeSetProvider : DataProvider, IChangeSetProvider
  {
    private readonly IChangeSetRepository _changeSetRepository;
    private readonly IChangeSetEntryPayloadMapper _payloadMapper;
    private readonly IDataConverter<ChangeSet, ChangeSetData> _changeSetDataConverter;

    private readonly IQueryable<ChangeSet> _changeSets;

    public ChangeSetProvider(IChangeSetRepository changeSetRepository, IChangeSetEntryPayloadMapper payloadMapper,
      DbContext context, IDataConverter<ChangeSet, ChangeSetData> changeSetDataConverter)
      : base(context)
    {
      _changeSetRepository = changeSetRepository;
      _payloadMapper = payloadMapper;
      _changeSetDataConverter = changeSetDataConverter;
      _changeSets = GetDataSource<ChangeSet>();
    }

    public async Task<IPagedList<ChangeSetData>> GetChangeSetPageAsync(ChangeSetPageRequest pageRequest,
      CancellationToken ct = default)
    {
      var query = _changeSets.Where(_ => _.Timestamp >= pageRequest.From && _.Timestamp <= pageRequest.To);

      if (!string.IsNullOrEmpty(pageRequest.UserId))
      {
        query = query.Where(_ => _.UpdatedBy == pageRequest.UserId);
      }

      if (pageRequest.ChangeType.HasValue)
      {
        query = query.Where(m => m.Entries.Any(_ => _.ChangeType == pageRequest.ChangeType));
      }

      var changeSetPage = await query
        .OrderByDescending(m => m.Timestamp)
        .PaginateAsync(pageRequest, ct);

      return changeSetPage.CopyWith(await _changeSetDataConverter.ConvertAsync(changeSetPage.Content, ct));
    }

    public async Task<ChangesetEntryPayloadData?> GetPayloadAsync(Guid changeSetEntryId,
      CancellationToken ct = default)
    {
      var current = await _changeSetRepository.GetEntryByIdAsync(changeSetEntryId, ct);

      if (current == null)
      {
        return null;
      }

      var currentPayload = await _payloadMapper.MapAsync(current, ct);
      if (currentPayload == null)
      {
        return null;
      }

      IDictionary<string, string?>? previous = null;
      if (current.ChangeType == ChangeType.Modification)
      {
        var prev = await _changeSetRepository.GetPreviousAsync(current, ct);
        previous = await _payloadMapper.MapAsync(prev!, ct);
      }

      return new ChangesetEntryPayloadData
      {
        Previous = previous,
        Current = currentPayload
      };
    }
  }
}