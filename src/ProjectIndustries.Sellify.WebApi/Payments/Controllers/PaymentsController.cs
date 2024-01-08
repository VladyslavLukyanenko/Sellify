using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Payments.Domain;
using ControllerBase = ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers.ControllerBase;

namespace ProjectIndustries.Sellify.WebApi.Payments.Controllers
{
  public class PaymentsController : ControllerBase
  {
    [HttpGet("Providers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<PaymentGateway[]>))]
    public IActionResult GetSupportedProvider()
    {
      var gateways = Enumeration.GetAll<PaymentGateway>();

      return Ok(gateways);
    }
  }
}