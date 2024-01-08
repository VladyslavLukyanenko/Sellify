using Microsoft.AspNetCore.Http;
using ProjectIndustries.Sellify.WebApi.Foundation.Audit;
using ProjectIndustries.Sellify.WebApi.Foundation.Authorization;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Services
{
  public class ClaimsPrincipalBasedIdentityProvider : IIdentityProvider
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClaimsPrincipalBasedIdentityProvider(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentIdentity()
    {
      return _httpContextAccessor.HttpContext?.User.GetUserId();
    }
  }
}