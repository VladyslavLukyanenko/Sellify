using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public interface IWebHookBindingRepository : ICrudRepository<WebHookBinding>
  {
    ValueTask<IList<WebHookBinding>> GetByTypeAsync(Guid storeId, string type, CancellationToken ct = default);
    ValueTask<WebHooksConfig?> GetConfigOfStoreAsync(Guid storeId, CancellationToken ct = default);
  }
}