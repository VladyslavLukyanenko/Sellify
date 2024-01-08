using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.Orders.Services
{
  public class CustomerPurchasesPageRequest : PageRequest
  {
    public long CustomerId { get; set; }
  }
}