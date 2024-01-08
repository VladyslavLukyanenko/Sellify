using System;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using ProjectIndustries.Sellify.App.Identity.Domain;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Services
{
  public class ProfileService : IProfileService
  {
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileService(IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
      UserManager<ApplicationUser> userManager)
    {
      _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
      _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
      string userSubject = context.Subject.GetSubjectId();
      var user = await _userManager.FindByIdAsync(userSubject);
      if (user == null)
      {
        return;
      }

      var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
      // context.AddRequestedClaims(principal.Claims);
      context.IssuedClaims.AddRange(principal.Claims);
      foreach (var claim in context.Subject.Claims)
      {
        if (!principal.HasClaim(claim.Type, claim.Value))
        {
          context.IssuedClaims.Add(claim);
        }
      }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
      string userSubject = context.Subject.GetSubjectId();
      var user = await _userManager.FindByIdAsync(userSubject);

#pragma warning disable CS8625
      context.IsActive = user != null
                         && (!user.LockoutEnd.HasValue || user.LockoutEnd < DateTimeOffset.Now)
                         /*&& user.EmailConfirmed*/;
#pragma warning restore
    }
  }
}