using System.Collections.Generic;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Mvc;

namespace ProjectIndustries.Sellify.WebApi.Controllers
{
  [ApiController]
  public class OidcConfigurationController : Controller
  {
    private readonly IClientRequestParametersProvider _clientRequestParametersProvider;

    public OidcConfigurationController(IClientRequestParametersProvider clientRequestParametersProvider)
    {
      _clientRequestParametersProvider = clientRequestParametersProvider;
    }

    [HttpGet("_configuration/{clientId}")]
    public IDictionary<string, string> GetClientRequestParameters([FromRoute] string clientId)
    {
      return _clientRequestParametersProvider.GetClientParameters(HttpContext, clientId);
    }
  }
}