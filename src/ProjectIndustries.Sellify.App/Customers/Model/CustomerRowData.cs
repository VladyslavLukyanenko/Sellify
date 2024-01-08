using NodaTime;

namespace ProjectIndustries.Sellify.App.Customers.Model
{
  public class CustomerRowData
  {
    public long Id { get; set; }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Email { get; set; } = null!;
    public decimal TotalSpent { get; set; }
    public int TotalOrdersCount { get; set; }
    public Instant? LastPurchasedAt { get; set; }
  }
}