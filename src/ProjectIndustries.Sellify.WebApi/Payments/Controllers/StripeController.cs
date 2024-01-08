using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc;
using ProjectIndustries.Sellify.WebApi.Payments.Services;
using ControllerBase = ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers.ControllerBase;

namespace ProjectIndustries.Sellify.WebApi.Payments.Controllers
{
  [ApiV1HttpRoute("Payments/[controller]")]
  public class StripeController : ControllerBase
  {
    private readonly IStripeGateway _stripeGateway;

    public StripeController(IStripeGateway stripeGateway)
    {
      _stripeGateway = stripeGateway;
    }

    [HttpPost("{orderId:guid}")]
    public async ValueTask<IActionResult> CreateSession(Guid orderId, CancellationToken ct)
    {
      var sessionId = await _stripeGateway.CreatePaymentSessionAsync(orderId, ct);
      if (sessionId == null)
      {
        return BadRequest();
      }

      return Ok(sessionId);
    }

    [HttpPut("{orderId:guid}/Cancel")]
    public async ValueTask<IActionResult> CancelOrderAsync(Guid orderId, CancellationToken ct)
    {
      await _stripeGateway.CancelSessionAsync(orderId, ct);
      return NoContent();
    }
  }
}