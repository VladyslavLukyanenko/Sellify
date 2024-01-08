using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.WebHooks.Data;
using ProjectIndustries.Sellify.Core.WebHooks;

namespace ProjectIndustries.Sellify.App.WebHooks.Services
{
  public interface IWebHookBindingService
  {
    ValueTask CreateAsync(Guid storeId, SaveBindingCommand cmd, CancellationToken ct = default);
    ValueTask UpdateAsync(WebHookBinding binding, SaveBindingCommand cmd, CancellationToken ct = default);
  }
}