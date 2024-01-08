using System;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndustries.Sellify.WebApi.Authorization;
using ProjectIndustries.Sellify.WebApi.Foundation.Authorization;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers
{
  public abstract class SecuredStoreBoundControllerBase : SecuredControllerBase
  {
    protected readonly IAppAuthorizationService AppAuthorizationService;
    private Guid? _storeId;

    protected SecuredStoreBoundControllerBase(IServiceProvider provider)
      : base(provider)
    {
      AppAuthorizationService = provider.GetRequiredService<IAppAuthorizationService>();
    }

    public Guid CurrentStoreId => (_storeId ??= User.GetCurrentStoreId()!).Value;
  }
}