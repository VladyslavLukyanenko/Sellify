using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.Audit.Services;

namespace ProjectIndustries.Sellify.Infra.Audit.EntryValueConverters
{
  public abstract class Int64ToStringEntryValueConverterBase : IChangeSetEntryValueConverter
  {
    public Task<string?> ConvertAsync(string? value, CancellationToken ct = default)
    {
      if (string.IsNullOrEmpty(value) || !long.TryParse(value, out var id))
      {
        return Task.FromResult<string?>(null);
      }

      return ConvertAsync(id, ct);
    }

    protected abstract Task<string?> ConvertAsync(long id, CancellationToken ct = default);
  }
}