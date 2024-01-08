using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.Services;
using ProjectIndustries.Sellify.Core.WebHooks.Discord;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public class DiscordWebHookPayloadFactory : IWebHookPayloadFactory
  {
    private readonly IWebhookDataSerializer _webhookDataSerializer;
    private readonly IJsonSerializer _jsonSerializer;

    public DiscordWebHookPayloadFactory(IWebhookDataSerializer webhookDataSerializer, IJsonSerializer jsonSerializer)
    {
      _webhookDataSerializer = webhookDataSerializer;
      _jsonSerializer = jsonSerializer;
    }

    public bool CanCreate(WebhookReceiverType receiverType) => receiverType == WebhookReceiverType.Discord;

    public async ValueTask<WebHookPayloadEnvelop> CreateAsync<T>(WebHookBinding binding, WebHooksConfig cfg, T data,
      CancellationToken ct = default)
      where T : WebHookDataBase
    {
      var serializedData = _webhookDataSerializer.Serialize(data);
      var discordPayload = new DiscordWebhookBody
      {
        Embeds =
        {
          new Embed
          {
            Title = binding.EventType,
            Fields = serializedData.Select(d => new Field
              {
                Name = Sanitize(d.Key),
                Value = Sanitize(d.Value)
              })
              .ToList()
          }
        }
      };

      var json = await _jsonSerializer.SerializeAsync(discordPayload, ct);
      return new WebHookPayloadEnvelop(json, cfg, binding.ListenerEndpoint);
    }

    private static string Sanitize(string? value) => string.IsNullOrEmpty(value) ? "-" : value;
  }
}