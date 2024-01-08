using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Infra.Identity.Services
{
  public interface IRolesProvider
  {
    ValueTask<IList<string>> GetRoleNamesAsync(long userId, CancellationToken ct = default);
  }
}