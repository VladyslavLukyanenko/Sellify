using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core.WebHooks
{
  public class WebhookReceiverType : Enumeration
  {
    public static readonly WebhookReceiverType CustomApi = new(1, nameof(CustomApi));
    public static readonly WebhookReceiverType Discord = new(2, nameof(Discord));
    public static readonly WebhookReceiverType Slack = new(3, nameof(Slack));
    // public static readonly WebhookReceiverType Telegram = new(4, nameof(Telegram));

    public WebhookReceiverType(int id, string name)
      : base(id, name)
    {
    }
  }
}