using System.Collections.Generic;
using ProjectIndustries.Sellify.Core.Products;

namespace ProjectIndustries.Sellify.App.Products.Model
{
  public class ProductData
  {
    public long Id { get; set; }
    public string SKU { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Excerpt { get; set; } = null!;
    public int Stock { get; set; }

    public ProductType Type { get; set; }

    // public Currency Currency { get; set; } = Currency.Usd;
    public decimal Price { get; set; }
    public long CategoryId { get; set; }

    public IList<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    public string? Picture { get; set; } = null!;
  }
}