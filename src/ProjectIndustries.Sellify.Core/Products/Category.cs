namespace ProjectIndustries.Sellify.Core.Products
{
  public class Category : StoreBoundEntity
  {
    public string Name { get; set; } = null!;
    public int Position { get; set; }

    public long? ParentCategoryId { get; private set; }
  }
}