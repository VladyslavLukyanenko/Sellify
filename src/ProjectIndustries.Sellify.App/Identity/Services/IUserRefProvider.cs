using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Identity.Model;
using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.Identity.Services
{
  public interface IUserRefProvider
  {
    ValueTask<UserRef> GetRefAsync(string userId, CancellationToken ct = default);
    ValueTask<IDictionary<string, UserRef>> GetRefsAsync(IEnumerable<string> userIds, CancellationToken ct = default);
    ValueTask<IPagedList<UserRef>> GetRefsPageAsync(UserRefsPageRequest pageRequest, CancellationToken ct);
  }
}