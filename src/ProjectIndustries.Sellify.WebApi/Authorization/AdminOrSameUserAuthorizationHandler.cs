using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ProjectIndustries.Sellify.WebApi.Foundation.Authorization;

namespace ProjectIndustries.Sellify.WebApi.Authorization
{
  public class AdminOrSameUserAuthorizationHandler : AuthorizationHandler<AdminOrSameUserRequirement>
  {
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
      AdminOrSameUserRequirement requirement)
    {
      if (context.User.HasAdminRole() || context.User.GetUserId() == requirement.UserId)
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}