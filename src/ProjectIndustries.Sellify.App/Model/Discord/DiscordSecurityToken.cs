#pragma warning disable 8618
using Newtonsoft.Json;

namespace ProjectIndustries.Sellify.App.Model.Discord
{
  public class DiscordSecurityToken
  {
    [JsonProperty("expires_in")] public int ExpiresIn { get; set; }
    [JsonProperty("access_token")] public string AccessToken { get; set; }
    [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
  }
}