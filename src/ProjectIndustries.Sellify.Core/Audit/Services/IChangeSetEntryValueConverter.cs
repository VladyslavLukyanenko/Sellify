using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.Audit.Services
{
  public interface IChangeSetEntryValueConverter
  {
    Task<string?> ConvertAsync(string? value, CancellationToken ct = default);
  }
}