using System.Threading.Tasks;
using MassTransit;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;

namespace ProjectIndustries.Sellify.Infra.WebHooks.Consumers
{
  public class WebHookSenderConsumer : IConsumer<WebHookPayloadEnvelop>
  {
    private readonly IWebHookSender _webHookSender;

    public WebHookSenderConsumer(IWebHookSender webHookSender)
    {
      _webHookSender = webHookSender;
    }

    public async Task Consume(ConsumeContext<WebHookPayloadEnvelop> context)
    {
      await _webHookSender.SendAsync(context.Message, context.CancellationToken);
    }
  }
}