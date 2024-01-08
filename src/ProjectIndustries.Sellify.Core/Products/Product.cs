using System;
using System.Collections.Generic;

namespace ProjectIndustries.Sellify.Core.Products
{
  public class Product : StoreBoundEntity
  {
    private List<ProductAttribute> _attributes = new();

    private Product()
    {
    }

    public Product(Guid storeId, string sku, string title, string content, string excerpt, ProductType type,
      decimal price, long categoryId, string? picture, int stock, IEnumerable<ProductAttribute> attributes)
    {
      StoreId = storeId;
      SKU = sku;
      Title = title;
      Content = content;
      Excerpt = excerpt;
      Type = type;
      Price = price;
      CategoryId = categoryId;
      Picture = picture;
      Stock = stock;

      _attributes.AddRange(attributes);
    }

    public string SKU { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string Excerpt { get; set; } = null!;
    public int Stock { get; set; }

    public ProductType Type { get; set; }

    // public Currency Currency { get; set; } = Currency.Usd;
    public decimal Price { get; set; }
    public long CategoryId { get; set; }
    public IReadOnlyList<ProductAttribute> Attributes => _attributes.AsReadOnly();
    public string? Picture { get; set; }

    public void ReplaceAttributes(IEnumerable<ProductAttribute> attributes)
    {
      _attributes.Clear();
      _attributes.AddRange(attributes);
    }
  }
}