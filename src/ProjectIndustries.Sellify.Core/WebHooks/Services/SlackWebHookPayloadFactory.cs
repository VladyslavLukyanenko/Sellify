using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectIndustries.Sellify.Core.Services;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public class SlackWebHookPayloadFactory : IWebHookPayloadFactory
  {
    private readonly IWebhookDataSerializer _webhookDataSerializer;
    private readonly IJsonSerializer _jsonSerializer;

    public SlackWebHookPayloadFactory(IWebhookDataSerializer webhookDataSerializer, IJsonSerializer jsonSerializer)
    {
      _webhookDataSerializer = webhookDataSerializer;
      _jsonSerializer = jsonSerializer;
    }

    public bool CanCreate(WebhookReceiverType receiverType) => receiverType == WebhookReceiverType.Slack;

    public async ValueTask<WebHookPayloadEnvelop> CreateAsync<T>(WebHookBinding binding, WebHooksConfig cfg, T data,
      CancellationToken ct = default) where T : WebHookDataBase
    {
      var payload = _webhookDataSerializer.Serialize(data);

      var message = new SlackMessage
      {
        Text = data.Type,
        Blocks =
        {
          new SlackTextHeader
          {
            Text =
            {
              Text = data.Type
            }
          }
        }
      };

      foreach (var pair in payload)
      {
        SlackTextField field;
        if (string.IsNullOrEmpty(pair.Key))
        {
          field = SlackTextField.CreateMarkdown(" ");
        }
        else
        {
          field = SlackTextField.CreateMarkdown($"*{pair.Key}:*\n{pair.Value}\n");
        }

        message.Blocks.Add(new SlackTextSection
        {
          Fields =
          {
            field
          }
        });
      }

      var json = await _jsonSerializer.SerializeAsync(message, ct);

      return new WebHookPayloadEnvelop(json, cfg, binding.ListenerEndpoint);
    }

    private class SlackMessage
    {
      [JsonProperty("text")] public string Text { get; set; } = null!;
      [JsonProperty("blocks")] public List<SlackMessageBlock> Blocks { get; set; } = new();
    }

    private abstract class SlackMessageBlock
    {
      [JsonProperty("type")] public abstract string Type { get; }
    }

    private class SlackTextHeader : SlackMessageBlock
    {
      public override string Type => "header";

      [JsonProperty("text")]
      public SlackTextField Text { get; set; } = new()
      {
        Type = "plain_text"
      };
    }

    private class SlackTextSection : SlackMessageBlock
    {
      public override string Type => "section";
      [JsonProperty("fields")] public List<SlackTextField> Fields { get; set; } = new();
    }

    private class SlackTextField
    {
      [JsonProperty("type")] public string Type { get; set; } = null!;
      [JsonProperty("text")] public string Text { get; set; } = null!;

      public static SlackTextField CreateMarkdown(string text) => new()
      {
        Type = "mrkdwn",
        Text = text
      };
    }
  }
}