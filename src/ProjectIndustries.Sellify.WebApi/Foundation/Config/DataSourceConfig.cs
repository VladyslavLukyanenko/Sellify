namespace ProjectIndustries.Sellify.WebApi.Foundation.Config
{
  public class DataSourceConfig
  {
    public string PostgresConnectionString { get; set; } = null!;

    public int MaxRetryCount { get; set; }
  }
}