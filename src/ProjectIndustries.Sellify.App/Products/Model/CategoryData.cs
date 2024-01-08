namespace ProjectIndustries.Sellify.App.Products.Model
{
  public class CategoryData
  {
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public int Position { get; set; }

    public long? ParentCategoryId { get; set; }
  }
}