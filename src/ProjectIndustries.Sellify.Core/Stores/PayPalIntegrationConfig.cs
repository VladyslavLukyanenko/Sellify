namespace ProjectIndustries.Sellify.Core.Stores
{
  public class PayPalIntegrationConfig
  {
    public string? Email { get; set; }
    public bool AcceptCreditCards { get; set; }
    public string? WebhookId { get; set; }

    public bool IsEmpty() => string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(WebhookId);
  }
}