namespace ProjectIndustries.Sellify.App.Products.Model
{
  public class ProductRowData
  {
    public long Id { get; set; }
    public string? Picture { get; set; }
    public string Title { get; set; } = null!;
    public string Excerpt { get; set; } = null!;
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; } = null!;
  }
}