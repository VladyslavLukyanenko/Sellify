using System;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core.Stores
{
  public class Store : SoftRemovableEntity<Guid>
  {
    private Store()
    {
    }

    public Store(string ownerId)
    {
      OwnerId = ownerId;
    }

    public string OwnerId { get; private set; } = null!;
    public string? Title { get; set; }
    public HostingConfig HostingConfig { get; private set; } = new();

    public PaymentGatewayIntegrationConfigs PaymentGatewayConfigs { get; private set; } = new();
  }
}