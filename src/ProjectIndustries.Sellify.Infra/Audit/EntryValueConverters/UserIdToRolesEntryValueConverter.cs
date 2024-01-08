using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Infra.Identity.Services;

namespace ProjectIndustries.Sellify.Infra.Audit.EntryValueConverters
{
  public class UserIdToRolesEntryValueConverter : Int64ToStringEntryValueConverterBase
  {
    private readonly IRolesProvider _rolesProvider;

    public UserIdToRolesEntryValueConverter(IRolesProvider rolesProvider)
    {
      _rolesProvider = rolesProvider;
    }

    protected override async Task<string?> ConvertAsync(long id, CancellationToken ct = default)
    {
      var roleNames = await _rolesProvider.GetRoleNamesAsync(id, ct);
      return string.Join(", ", roleNames);
    }
  }
}