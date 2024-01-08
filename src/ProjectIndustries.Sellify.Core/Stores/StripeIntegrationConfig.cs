namespace ProjectIndustries.Sellify.Core.Stores
{
  public class StripeIntegrationConfig
  {
    public string? ApiKey { get; set; }
    public string? WebhookSecret { get; set; }

    public bool IsEmpty() => string.IsNullOrEmpty(ApiKey) || string.IsNullOrEmpty(WebhookSecret);
  }
}