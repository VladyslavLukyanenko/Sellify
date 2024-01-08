namespace ProjectIndustries.Sellify.Core.Stores
{
  public class SkrillIntegrationConfig
  {
    public string? Email { get; set; }
    public string? Secret { get; set; }

    public bool IsEmpty() => string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Secret);
  }
}