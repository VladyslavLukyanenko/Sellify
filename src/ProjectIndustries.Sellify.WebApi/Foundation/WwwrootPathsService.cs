using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using ProjectIndustries.Sellify.App.Config;
using ProjectIndustries.Sellify.App.Services;

namespace ProjectIndustries.Sellify.WebApi.Foundation
{
  public class WwwrootPathsService
    : IPathsService
  {
    private readonly CommonConfig _commonConfig;
    private readonly IWebHostEnvironment _environment;

    public WwwrootPathsService(IWebHostEnvironment environment,
      IOptions<CommonConfig> commonConfigOptions)
    {
      _environment = environment;
      _commonConfig = commonConfigOptions.Value;
    }

    public string? ToAbsoluteUrl(string? path)
    {
      if (string.IsNullOrEmpty(path))
      {
        return null;
      }

      var hostWithScheme = _commonConfig.HostingInfo.HostName;
      if (!path.StartsWith("/") && !hostWithScheme.EndsWith("/"))
      {
        hostWithScheme += "/";
      }

      return hostWithScheme + path;
    }

    public string GetStoreAbsolutePath(params string[] segments)
    {
      var allSegments = new List<string>();
      if (!_commonConfig.Uploads.IsPathRelative)
      {
        allSegments.Add(_commonConfig.Uploads.DirectoryName);
      }

      allSegments.AddRange(segments.Select(SanitizePathSegment));

      var storePath = Path.Combine(allSegments.ToArray());
      if (Path.IsPathRooted(storePath))
      {
        return storePath;
      }

      return Path.Combine(_environment.WebRootPath, storePath);
    }

    public string GetAbsolutePathFromUrl(string url)
    {
      var uri = new Uri(url);
      var path = uri.AbsolutePath;
      var info = _environment.WebRootFileProvider.GetFileInfo(path);

      return info.PhysicalPath;
    }

    public string GetPhysicalPath(string path)
    {
      var relativePath = SanitizePathSegment(path);

      return Path.Combine(_environment.WebRootPath, relativePath);
    }

    public string ToServerRelative(string path)
    {
      var relativePath = path.Replace(_environment.WebRootPath, string.Empty);
      var normalizedRelativePath = relativePath.Replace(Path.DirectorySeparatorChar, '/');
      if (!normalizedRelativePath.StartsWith("/"))
      {
        normalizedRelativePath = "/" + normalizedRelativePath;
      }

      return normalizedRelativePath;
    }

    private static string SanitizePathSegment(string relativePath)
    {
      if (relativePath.StartsWith("/"))
      {
        relativePath = relativePath.Substring(1);
      }

      return relativePath;
    }
  }
}