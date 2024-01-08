using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProjectIndustries.Sellify.WebApi.Authorization
{
  public interface IAppAuthorizationService
  {
    ValueTask<AuthorizationResult> AdminOrMemberAsync(Guid storeId);
    ValueTask<AuthorizationResult> AdminOrSameUserAsync(long userId);
    // ValueTask<AuthorizationResult> AdminOrDashboardOwnerAsync(Guid storeId);
    ValueTask<AuthorizationResult> AuthorizeCurrentPermissionsAsync(Guid storeId);
  }

  public class AppAuthorizationService : IAppAuthorizationService
  {
    public ValueTask<AuthorizationResult> AdminOrMemberAsync(Guid storeId)
    {
      throw new NotImplementedException();
    }

    public ValueTask<AuthorizationResult> AdminOrSameUserAsync(long userId)
    {
      throw new NotImplementedException();
    }

    public ValueTask<AuthorizationResult> AuthorizeCurrentPermissionsAsync(Guid storeId)
    {
      throw new NotImplementedException();
    }
  }
}