namespace ProjectIndustries.Sellify.App.Identity.Model
{
  public class UserRef
  {
    public UserRef()
    {
    }

    public UserRef(string id)
    {
      Id = id;
    }

    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Picture { get; set; }
  }
}