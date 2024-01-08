using NodaTime;
using ProjectIndustries.Sellify.App.Model;

namespace ProjectIndustries.Sellify.App.Customers.Services
{
  public class CustomerRowPageRequest : FilteredPageRequest
  {
    // todo: refactor it. this is duplicate
    public Instant? StartAt { get; set; }
    public Instant? EndAt { get; set; }

    public bool IsFilteredByTimePeriod() => StartAt.HasValue && EndAt.HasValue;
  }
}