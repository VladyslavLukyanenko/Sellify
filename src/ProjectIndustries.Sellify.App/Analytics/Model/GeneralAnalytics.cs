using System.Collections.Generic;
using NodaTime;

namespace ProjectIndustries.Sellify.App.Analytics.Model
{
  public class GeneralAnalytics
  {
    public Instant AggregatedAt { get; set; } = SystemClock.Instance.GetCurrentInstant();

    public ValueDiff<decimal> TotalRevenue { get; set; } = null!;
    public ValueDiff<int> TotalOrders { get; set; } = null!;
    public ValueDiff<int> TotalCustomers { get; set; } = null!;
    public ValueDiff<int> VisitorsCount { get; set; } = null!;

    public IList<IncomeStatsItem> Income { get; set; } = new List<IncomeStatsItem>();
  }
}