namespace ProjectIndustries.Sellify.App.Services
{
  public interface IPathsService
  {
    string ToServerRelative(string path);
    string? ToAbsoluteUrl(string? path);
    string GetStoreAbsolutePath(params string[] segments);
    string GetAbsolutePathFromUrl(string url);
    string GetPhysicalPath(string path);
  }
}