namespace ProjectIndustries.Sellify.Core.Primitives
{
  public interface IAuthorAuditable
  {
    string? UpdatedBy { get; }
    string? CreatedBy { get; }
  }
}