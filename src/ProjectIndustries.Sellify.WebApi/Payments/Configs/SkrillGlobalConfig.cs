namespace ProjectIndustries.Sellify.WebApi.Payments.Configs
{
  public class SkrillGlobalConfig
  {
    public string MQI { get; set; } = null!;
    public string CreateSessionEndpoint { get; set; } = null!;
    public string QuickCheckoutUrlTemplate { get; set; } = null!;

    public string BuildQuickCheckoutUrl(string sessionId) => string.Format(QuickCheckoutUrlTemplate, sessionId);
  }
}