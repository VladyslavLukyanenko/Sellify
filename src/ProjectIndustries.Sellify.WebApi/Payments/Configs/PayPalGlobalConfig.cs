namespace ProjectIndustries.Sellify.WebApi.Payments.Configs
{
  public class PayPalGlobalConfig
  {
    public string AccountId { get; set; } = null!;
    public string ClientId { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string Mode { get; set; } = "sandbox";
  }
}