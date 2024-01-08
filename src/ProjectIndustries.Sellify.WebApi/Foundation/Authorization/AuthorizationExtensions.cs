using System;
using System.Security.Claims;
using System.Security.Principal;
using IdentityModel;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Authorization
{
  public static class AuthorizationExtensions
  {
    public static bool HasAdminRole(this IPrincipal self) => self.IsInRole(SupportedRoleNames.Administrator);

    public static string? GetUserId(this ClaimsPrincipal self)
    {
      return self.FindFirst(JwtClaimTypes.Id)?.Value;
    }

    public static Guid? GetCurrentStoreId(this ClaimsPrincipal self)
    {
      var rawStoreId = self.FindFirst(AppClaimNames.StoreId)?.Value;
      if (Guid.TryParse(rawStoreId, out var storeId))
      {
        return storeId;
      }

      return null;
    }
  }
}