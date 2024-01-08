using System;
using NodaTime;
using ProjectIndustries.Sellify.Core.Orders;

namespace ProjectIndustries.Sellify.App.Orders.Model
{
  public class PurchasedProductData
  {
    public Guid OrderId { get; set; }
    public long ProductId { get; set; }
    public decimal Price { get; set; }
    public string Title { get; set; } = null!;
    public uint Quantity { get; set; }
    public Instant? PaidAt { get; set; }
    public OrderStatus OrderStatus { get; set; }
  }
}