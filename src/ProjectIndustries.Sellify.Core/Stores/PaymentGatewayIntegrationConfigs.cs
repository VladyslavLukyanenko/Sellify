namespace ProjectIndustries.Sellify.Core.Stores
{
  public class PaymentGatewayIntegrationConfigs
  {
    public StripeIntegrationConfig Stripe { get; private set; } = new();
    public SkrillIntegrationConfig Skrill { get; private set; } = new();
    public PayPalIntegrationConfig PayPal { get; private set; } = new();
  }
}