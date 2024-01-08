using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.Stores
{
  public interface IStoreRepository
  {
    ValueTask<Store?> GetByIdAsync(Guid storeId, CancellationToken ct = default);
    ValueTask<Store?> GetByOwnerIdAsync(string userId, CancellationToken ct = default);
    ValueTask<Store> CreateAsync(Store store, CancellationToken ct = default);
    ValueTask<Store?> GetByRawLocationAsync(string url, CancellationToken ct = default);
  }
}