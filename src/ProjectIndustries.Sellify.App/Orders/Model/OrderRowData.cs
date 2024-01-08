using System;
using NodaTime;
using ProjectIndustries.Sellify.Core.Orders;

namespace ProjectIndustries.Sellify.App.Orders.Model
{
  public class OrderRowData
  {
    public Guid Id { get; set; }
    public Instant CreatedAt { get; set; }
    public string? CustomerEmail { get; set; }

    public string InvoiceEmail { get; set; } = null!;
    public string? CustomerName { get; set; }
    // public int Currency { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public PurchasedProductData PurchasedProduct { get; set; } = null!;
  }
}