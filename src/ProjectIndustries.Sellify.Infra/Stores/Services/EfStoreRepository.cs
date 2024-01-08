using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.Core.Stores.Config;
using ProjectIndustries.Sellify.Infra.Repositories;

namespace ProjectIndustries.Sellify.Infra.Stores.Services
{
  public class EfStoreRepository : EfSoftRemovableCrudRepository<Store, Guid>, IStoreRepository
  {
    private readonly StoresConfig _storesConfig;
    public EfStoreRepository(DbContext context, IUnitOfWork unitOfWork, StoresConfig storesConfig)
      : base(context, unitOfWork)
    {
      _storesConfig = storesConfig;
    }

    public async ValueTask<Store?> GetByOwnerIdAsync(string userId, CancellationToken ct = default)
    {
      return await DataSource.FirstOrDefaultAsync(_ => _.OwnerId == userId, ct);
    }

    public async ValueTask<Store?> GetByRawLocationAsync(string url, CancellationToken ct = default)
    {
      var modes = HostingConfig.ResolvePossibleModes(url, _storesConfig);
      if (modes.IsFailure)
      {
        return null;
      }

      var normalizedModes = modes.Value.Select(m => $"{(int) m.Key}__{m.Value.ToLower()}");
      return await DataSource.FirstOrDefaultAsync(
        d => d.HostingConfig.DomainName != null
             && normalizedModes.Contains(d.HostingConfig.Mode + "__" + d.HostingConfig.DomainName.ToLower()), ct);
    }
  }
}