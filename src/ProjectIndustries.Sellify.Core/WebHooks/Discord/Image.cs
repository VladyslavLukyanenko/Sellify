using System.Reflection;
using Newtonsoft.Json;

namespace ProjectIndustries.Sellify.Core.WebHooks.Discord
{
  [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
  public class Image
  {
    [JsonProperty("url")] public string Url { get; set; } = "";
  }
}