namespace ProjectIndustries.Sellify.App.Orders.Model
{
  public class CreatePayPayOrderCommand
  {
    public string OrderId { get; set; } = null!;
    public string ProductTitle { get; set; } = null!;
    public string ProductDesc { get; set; } = null!;
    public string TotalPrice { get; set; } = null!;
    public string PricePerItem { get; set; } = null!;
    public string ProductSKU { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public string Quantity { get; set; } = null!;
    public string PayeeEmail { get; set; } = null!;
    public string PayerEmail { get; set; } = null!;
    public string StoreTitle { get; set; } = null!;
  }
}