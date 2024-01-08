using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.Core.Stores.Config;

namespace ProjectIndustries.Sellify.Core.Stores
{
  public class HostingConfig
  {
    public string? DomainName { get; set; }
    public HostingMode Mode { get; set; }

    public static Result<IDictionary<HostingMode, string>> ResolvePossibleModes(
      string rawLocation, StoresConfig config)
    {
      if (!Uri.TryCreate(rawLocation, UriKind.RelativeOrAbsolute, out var url)
          || !url.IsAbsoluteUri
          || string.IsNullOrEmpty(url.AbsolutePath))
      {
        return Result.Failure<IDictionary<HostingMode, string>>("Invalid store url provided");
      }

      var result = new Dictionary<HostingMode, string>();
      var segmentMatch = Regex.Match(url.AbsolutePath, config.LocationPathSegmentRegex);
      if (segmentMatch.Success)
      {
        result[HostingMode.PathSegment] = segmentMatch.Groups[1].Value;
      }

      if (url.HostNameType == UriHostNameType.Dns)
      {
        if (url.Host.Count(c => c == '.') > 1)
        {
          var dotIDx = url.Host.IndexOf('.');
          result[HostingMode.Subdomain] = url.Host[..dotIDx];
        }
        // else
        // {
        //   result[HostingMode.Dedicated] = url.Host;
        // }
      }

      return result;
    }
  }
}