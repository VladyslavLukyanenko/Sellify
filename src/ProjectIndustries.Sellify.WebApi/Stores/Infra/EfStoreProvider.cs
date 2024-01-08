using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.App.Stores.Model;
using ProjectIndustries.Sellify.App.Stores.Services;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.Core.Stores.Config;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.WebApi.Stores.Infra
{
  public class EfStoreProvider : DataProvider, IStoreProvider
  {
    private readonly IQueryable<Store> _stores;
    private readonly StoresConfig _config;

    public EfStoreProvider(DbContext context, StoresConfig config) : base(context)
    {
      _config = config;
      _stores = GetAliveDataSource<Store>();
    }

    public async ValueTask<StoreInfoData?> GetStoreInfoAsync(string slug, CancellationToken ct = default)
    {
      return await _stores.Where(_ => _.HostingConfig.DomainName == slug)
        .Select(s => new StoreInfoData
        {
          Id = s.Id,
          Title = s.Title
        })
        .FirstOrDefaultAsync(ct);
    }
  }
}