using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.WebHooks.Data;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;

namespace ProjectIndustries.Sellify.App.WebHooks.Services
{
  public class WebHookBindingService : IWebHookBindingService
  {
    private readonly IWebHookBindingRepository _webHookBindingRepository;

    public WebHookBindingService(IWebHookBindingRepository webHookBindingRepository)
    {
      _webHookBindingRepository = webHookBindingRepository;
    }

    public async ValueTask CreateAsync(Guid storeId, SaveBindingCommand cmd, CancellationToken ct = default)
    {
      var receiverType = cmd.ReceiverType.ToEnumeration<WebhookReceiverType>();
      var binding = new WebHookBinding(cmd.EventType, cmd.ListenerEndpoint, storeId, receiverType);
      await _webHookBindingRepository.CreateAsync(binding, ct);
    }

    public ValueTask UpdateAsync(WebHookBinding binding, SaveBindingCommand cmd, CancellationToken ct = default)
    {
      binding.EventType = cmd.EventType;
      binding.ListenerEndpoint = cmd.ListenerEndpoint;
      binding.ReceiverType = cmd.ReceiverType.ToEnumeration<WebhookReceiverType>();

      return default;
    }
  }
}