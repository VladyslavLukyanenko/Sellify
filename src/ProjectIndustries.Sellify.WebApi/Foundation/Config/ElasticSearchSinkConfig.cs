#pragma warning disable 8618

namespace ProjectIndustries.Sellify.WebApi.Foundation.Config
{
  public class ElasticSearchSinkConfig
  {
    public bool IsEnabled { get; set; }
    public string CloudId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
  }
}