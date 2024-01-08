namespace ProjectIndustries.Sellify.Core.Products
{
  public class ProductAttribute
  {
    public string Name { get; set; } = null!;
    public string Value { get; set; } = null!;
    public AttributeType Type { get; set; }
  }
}