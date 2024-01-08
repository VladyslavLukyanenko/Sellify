using System;
using ProjectIndustries.Sellify.App.Identity.Model;

namespace ProjectIndustries.Sellify.App.Commands
{
  public class CreateWithConfirmedEmailCommand : UserData
  {
    public string[] RoleNames { get; set; } = Array.Empty<string>();
    public string Password { get; set; } = null!;
  }
}