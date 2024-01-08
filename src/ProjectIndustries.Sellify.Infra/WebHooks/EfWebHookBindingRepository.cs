using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;
using ProjectIndustries.Sellify.Infra.Repositories;

namespace ProjectIndustries.Sellify.Infra.WebHooks
{
  public class EfWebHookBindingRepository : EfCrudRepository<WebHookBinding>, IWebHookBindingRepository
  {
    public EfWebHookBindingRepository(DbContext context, IUnitOfWork unitOfWork)
      : base(context, unitOfWork)
    {
    }

    public async ValueTask<IList<WebHookBinding>> GetByTypeAsync(Guid storeId, string type, CancellationToken ct = default)
    {
      return await DataSource.Where(_ => _.StoreId == storeId && _.EventType == type)
        .ToListAsync(ct);
    }

    public async ValueTask<WebHooksConfig?> GetConfigOfStoreAsync(Guid storeId, CancellationToken ct = default)
    {
      return await Context.Set<WebHooksConfig>()
        .AsQueryable()
        .FirstOrDefaultAsync(_ => _.StoreId == storeId, ct);
    }
  }
}