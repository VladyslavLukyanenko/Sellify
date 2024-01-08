using NodaTime;

namespace ProjectIndustries.Sellify.App.Analytics.Model
{
  public class IncomeStatsItem
  {
    public decimal Amount { get; set; } = new();
    public LocalDate Date { get; set; }
  }
}