using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.Core.Audit.Services;

namespace ProjectIndustries.Sellify.Infra.Audit.EntryValueConverters
{
  public class UserIdToFullNameEntryValueConverter : IChangeSetEntryValueConverter
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public UserIdToFullNameEntryValueConverter(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public async Task<string?> ConvertAsync(string? id, CancellationToken ct = default)
    {
      var user = await _userManager.FindByIdAsync(id);
      return user?.UserName;
    }
  }
}