using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Infra.Converters
{
  public interface IDataConverter<in TSource, TDst>
  {
    Task<TDst> ConvertAsync(TSource src, CancellationToken ct = default);
    Task<IList<TDst>> ConvertAsync(IEnumerable<TSource> src, CancellationToken ct = default);
  }
}