using System;

namespace ProjectIndustries.Sellify.Core.Customers
{
  public class Customer : StoreBoundEntity
  {
    private Customer()
    {
    }

    public Customer(Guid storeId, string email)
    {
      StoreId = storeId;
      Email = email;
    }

    public string Email { get; private set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
  }
}