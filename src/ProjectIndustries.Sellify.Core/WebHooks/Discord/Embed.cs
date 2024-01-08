using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ProjectIndustries.Sellify.Core.WebHooks.Discord
{
  [Obfuscation(Feature = "renaming", ApplyToMembers = true)]
  public partial class Embed
  {
    private const string SuccessTitle = "**Successful Submit** :partying_face:";
    private const int SuccessColor = 0x32CD32;

    private const string FailureTitle = "**Submit Failed** :frowning:";
    private const int FailureColor = 0xCC0000;

    private const string ExpiredTitle = "**Raffle Expired** :frowning:";
    private const int ExpiredColor = 0x999999;

    [JsonProperty("author")] public Author Author { get; set; } = new();

    [JsonProperty("color")] public long Color { get; set; } = SuccessColor;

    [JsonProperty("title")] public string Title { get; set; } = null!;

    [JsonProperty("url")] public string Url { get; set; } = "";

    [JsonProperty("image")] public Image Image { get; set; } = new();

    [JsonProperty("thumbnail")] public Image Thumbnail { get; set; } = new();

    [JsonProperty("footer")] public Footer Footer { get; set; } = new();

    [JsonProperty("fields")] public List<Field> Fields { get; set; } = new();
  }
}