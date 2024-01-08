using System.Collections.Generic;

namespace ProjectIndustries.Sellify.App.Model.Discord
{
  public class GuildMember
  {
    public List<ulong> Roles { get; set; } = new();
  }
}