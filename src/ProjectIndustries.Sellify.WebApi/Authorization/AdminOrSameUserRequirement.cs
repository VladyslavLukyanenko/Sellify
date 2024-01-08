using Microsoft.AspNetCore.Authorization;

namespace ProjectIndustries.Sellify.WebApi.Authorization
{
  public class AdminOrSameUserRequirement : IAuthorizationRequirement
  {
    public AdminOrSameUserRequirement(string userId)
    {
      UserId = userId;
    }

    public string UserId { get; }
  }
}