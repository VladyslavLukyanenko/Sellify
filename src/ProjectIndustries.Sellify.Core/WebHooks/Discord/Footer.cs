using System.Reflection;
using Newtonsoft.Json;

namespace ProjectIndustries.Sellify.Core.WebHooks.Discord
{
  public class Footer
  {
    [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
    [JsonProperty("text")]
    public string Text { get; set; } = null!;

    [JsonProperty("icon_url")]
    public string IconUrl { get; set; } = "https://projectindustries.gg/images/logo-project-raffles.png";
  }
}