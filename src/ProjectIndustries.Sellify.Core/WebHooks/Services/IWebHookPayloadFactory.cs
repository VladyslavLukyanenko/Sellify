using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public interface IWebHookPayloadFactory
  {
    bool CanCreate(WebhookReceiverType receiverType);

    ValueTask<WebHookPayloadEnvelop> CreateAsync<T>(WebHookBinding binding, WebHooksConfig cfg, T data,
      CancellationToken ct = default)
      where T : WebHookDataBase;
  }
}