using System.Reflection;
using Newtonsoft.Json;

namespace ProjectIndustries.Sellify.Core.WebHooks.Discord
{
  [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
  public class Field
  {
    [JsonProperty("name")] public string Name { get; set; } = null!;

    [JsonProperty("value")] public string Value { get; set; } = null!;

    [JsonProperty("inline")] public bool Inline { get; set; }
  }
}