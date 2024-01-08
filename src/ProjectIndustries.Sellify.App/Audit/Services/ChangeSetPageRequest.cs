using NodaTime;
using ProjectIndustries.Sellify.Core.Audit;
using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.Audit.Services
{
  public class ChangeSetPageRequest : PageRequest
  {
    public string? UserId { get; set; }
    public ChangeType? ChangeType { get; set; } = null!;

    public Instant From { get; set; }
    public Instant To { get; set; }
  }
}