using System;
using System.Collections.Generic;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Config
{
  public class IdentityConfig
  {
    public IList<ClientConfig> Clients { get; set; } = new List<ClientConfig>();

    public class ClientConfig
    {
      public string Id { get; set; } = null!;
      public string Name { get; set; } = null!;

      public TimeSpan AccessTokenLifetime { get; set; }
      public TimeSpan RefreshTokenLifetime { get; set; }
      public string ApiSecret { get; set; } = null!;
    }
  }
}