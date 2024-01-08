using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Stores.Model;

namespace ProjectIndustries.Sellify.App.Stores.Services
{
  public interface IStoreProvider
  {
    ValueTask<StoreInfoData?> GetStoreInfoAsync(string slug, CancellationToken ct = default);
  }
}