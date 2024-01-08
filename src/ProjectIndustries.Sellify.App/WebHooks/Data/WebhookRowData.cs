using NodaTime;

namespace ProjectIndustries.Sellify.App.WebHooks.Data
{
  public class WebhookRowData
  {
    public long Id { get; set; }
    public string ReceiverType { get; set; } = null!;
    public string EventType { get; set; } = null!;
    public string ListenerEndpoint { get; set; } = null!;
    public Instant CreatedAt { get; set; }
  }
}