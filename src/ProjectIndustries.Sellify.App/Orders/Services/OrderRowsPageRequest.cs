using NodaTime;
using ProjectIndustries.Sellify.App.Model;
using ProjectIndustries.Sellify.Core.Orders;

namespace ProjectIndustries.Sellify.App.Orders.Services
{
  public class OrderRowsPageRequest : FilteredPageRequest
  {
    public OrderStatus? ExpectedStatus { get; set; }
    public Instant? StartAt { get; set; }
    public Instant? EndAt { get; set; }

    public bool IsFilteredByTimePeriod() => StartAt.HasValue && EndAt.HasValue;
  }
}