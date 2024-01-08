using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ProjectIndustries.Sellify.App.Audit.Data;
using ProjectIndustries.Sellify.App.Identity.Services;
using ProjectIndustries.Sellify.Core.Audit;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.Infra.Audit.DataConverters
{
  public class ChangeSetDataConverter : DataConverterBase<ChangeSet, ChangeSetData>
  {
    private readonly IUserRefProvider _userRefProvider;
    private readonly IMapper _mapper;

    public ChangeSetDataConverter(IUserRefProvider userRefProvider, IMapper mapper)
    {
      _userRefProvider = userRefProvider;
      _mapper = mapper;
    }

    public override async Task<IList<ChangeSetData>> ConvertAsync(IEnumerable<ChangeSet> src,
      CancellationToken ct = default)
    {
      var list = _mapper.Map<List<ChangeSetData>>(src);

      await _userRefProvider.InitializeAsync(list.Select(_ => _.UpdatedBy), ct);

      return list;
    }
  }
}