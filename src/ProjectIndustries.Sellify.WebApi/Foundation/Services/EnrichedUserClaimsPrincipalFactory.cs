using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Foundation.Authorization;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Services
{
  public class EnrichedUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
  {
    private readonly IStoreRepository _storeRepository;

    public EnrichedUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
      RoleManager<ApplicationRole> roleManager, IOptions<IdentityOptions> options, IStoreRepository storeRepository)
      : base(userManager, roleManager, options)
    {
      _storeRepository = storeRepository;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
      var identity = await base.GenerateClaimsAsync(user);
      identity.AddClaim(new Claim(JwtClaimTypes.Id, user.Id));
      identity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.FirstName));
      identity.AddClaim(new Claim(JwtClaimTypes.FamilyName, user.LastName));

      if (!string.IsNullOrEmpty(user.Picture))
      {
        identity.AddClaim(new Claim(JwtClaimTypes.Picture, user.Picture));
      }

      var ownStore = await _storeRepository.GetByOwnerIdAsync(user.Id);
      if (ownStore != null)
      {
        identity.AddClaim(new Claim(AppClaimNames.StoreId, ownStore.Id.ToString()));
      }

      return identity;
    }
  }
}