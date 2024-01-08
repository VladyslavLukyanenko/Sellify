namespace ProjectIndustries.Sellify.Core.Stores.Config
{
  public class StoresConfig
  {
    public string[] BlacklistedDomains { get; set; } = null!;
    public string LocationPathSegmentRegex { get; set; } = null!;
  }
}