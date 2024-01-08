using ProjectIndustries.Sellify.Core.Products;

namespace ProjectIndustries.Sellify.Core.Orders
{
  public class PurchasedProduct
  {
    internal PurchasedProduct()
    {
    }

    public PurchasedProduct(Product product, uint quantity)
    {
      Id = product.Id;
      Title = product.Title;
      Picture = product.Picture;
      Price = product.Price;
      Quantity = quantity;
    }

    public long Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Picture { get; private set; }
    public decimal Price { get; private set; }
    public uint Quantity { get; private set; }

    public decimal CalculateTotalPrice() => Price * Quantity;
  }
}