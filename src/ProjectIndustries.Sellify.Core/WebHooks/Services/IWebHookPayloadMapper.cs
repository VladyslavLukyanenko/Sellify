using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public interface IWebHookPayloadMapper
  {
    bool CanMap(object @event);

    ValueTask<WebHookDataBase?> MapAsync(object @event, CancellationToken ct = default);
  }
}