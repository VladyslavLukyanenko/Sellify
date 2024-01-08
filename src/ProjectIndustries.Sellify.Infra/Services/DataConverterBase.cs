using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Infra.Converters;

namespace ProjectIndustries.Sellify.Infra.Services
{
  public abstract class DataConverterBase<TSource, TDst> : IDataConverter<TSource, TDst>
  {
    public async Task<TDst> ConvertAsync(TSource src, CancellationToken ct = default)
    {
      var list = await ConvertAsync(new[] {src}, ct);
      return list[0];
    }

    public abstract Task<IList<TDst>> ConvertAsync(IEnumerable<TSource> src, CancellationToken ct = default);
  }
}