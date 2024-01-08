using ProjectIndustries.Sellify.Core.Stores;

namespace ProjectIndustries.Sellify.WebApi.Payments.Configs
{
  public class StripeGlobalConfig
  {
    public const string StoreNamePlaceholder = "{StoreName}";

    public string RedirectUrlTemplate { get; set; } = null!;

    public string GetPaymentCancelledUrl(Store store) =>
      RedirectUrlTemplate.Replace(StoreNamePlaceholder, store.HostingConfig.DomainName) + "/cancelled";

    public string GetPaymentSuccessfulUrl(Store store) =>
      RedirectUrlTemplate.Replace(StoreNamePlaceholder, store.HostingConfig.DomainName) + "/successful";
  }
}