using ProjectIndustries.Sellify.App.Services;

namespace ProjectIndustries.Sellify.Infra
{
  // ReSharper disable once InconsistentNaming
  public static class IPathsServiceExtensions
  {
    public static string? GetAbsoluteImageUrl(this IPathsService pathsService, string? relativePath,
      string? fallbackRelativePath)
    {
      if (string.IsNullOrEmpty(relativePath))
      {
        relativePath = fallbackRelativePath;
      }

      return pathsService.ToAbsoluteUrl(relativePath);
    }
  }
}