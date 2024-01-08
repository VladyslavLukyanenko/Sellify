using ProjectIndustries.Sellify.App.Identity.Model;

namespace ProjectIndustries.Sellify.App.Commands
{
  public class RegisterWithEmailCommand : UserData
  {
    public string Password { get; set; } = null!;
  }
}