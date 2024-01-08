using System;

namespace ProjectIndustries.Sellify.Core.Orders.Services
{
  public class CreateOrderCommand
  {
    public Guid StoreId { get; set; }
    public long ProductId { get; set; }
    public string CustomerEmail { get; set; } = null!;
    public uint Quantity { get; set; }
  }
}