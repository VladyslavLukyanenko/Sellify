using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.WebApi.Foundation.Authorization;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers
{
  [Authorize]
  [ProducesResponseType(StatusCodes.Status401Unauthorized)]
  [ProducesResponseType(StatusCodes.Status403Forbidden)]
  public abstract class SecuredControllerBase : ControllerBase
  {
    private string? _currentUserId;

    protected SecuredControllerBase(IServiceProvider provider)
    {
    }

    protected string? CurrentUserId => _currentUserId ??= User.GetUserId();
  }
}