using System;
using NodaTime;
using ProjectIndustries.Sellify.Core.Audit;

namespace ProjectIndustries.Sellify.App.Audit.Data
{
  public class ChangeSetEntryRefData
  {
    public Guid Id { get; set; }
    public Instant CreatedAt { get; set; }
    public string EntityId { get; set; } = null!;
    public string EntityType { get; set; } = null!;
    public ChangeType ChangeType { get; set; }
  }
}