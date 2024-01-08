using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc;
using ProjectIndustries.Sellify.WebApi.Payments.Services.Stripe;
using Stripe;
using ControllerBase = ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers.ControllerBase;

namespace ProjectIndustries.Sellify.WebApi.Payments.Controllers
{
  [ApiV1HttpRoute("Payments/{storeId:guid}/Stripe/Hooks")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class StripeWebHooksController : ControllerBase
  {
    private readonly IStoreRepository _storeRepository;
    private readonly IEnumerable<IStripeWebHookHandler> _webHookHandlers;
    private readonly ILogger<StripeWebHooksController> _logger;

    public StripeWebHooksController(IStoreRepository storeRepository,
      IEnumerable<IStripeWebHookHandler> webHookHandlers, ILogger<StripeWebHooksController> logger)
    {
      _storeRepository = storeRepository;
      _webHookHandlers = webHookHandlers;
      _logger = logger;
    }

    [HttpPost]
    public async ValueTask<IActionResult> HandleWebhookAsync([FromHeader(Name = "Stripe-Signature")]
      string signature, Guid storeId, CancellationToken ct)
    {
      string rawPayload;
      using (var reader = new StreamReader(Request.Body))
      {
        rawPayload = await reader.ReadToEndAsync();
      }

      try
      {
        _logger.LogDebug("Received stripe webhook. Searching for store with Id {StoreId}", storeId);
        var store = await _storeRepository.GetByIdAsync(storeId, ct);
        if (store == null)
        {
          _logger.LogWarning("Store {StoreId} not found", storeId);
          return NotFound();
        }

        if (store.PaymentGatewayConfigs.Stripe.IsEmpty())
        {
          return BadRequest(("Stripe integration not configured for store " + storeId).ToApiError());
        }

        _logger.LogDebug("Constructing event");
        var @event =
          EventUtility.ConstructEvent(rawPayload, signature, store.PaymentGatewayConfigs.Stripe.WebhookSecret);
        _logger.LogDebug("Event {EventType} constructed", @event.Type);

        var handler = _webHookHandlers.FirstOrDefault(_ => _.CanHandle(@event.Type));
        if (handler == null)
        {
          _logger.LogWarning("Not handler for event {EventType}", @event.Type);
          return NoContent();
        }

        _logger.LogDebug("Handling event {EventType}", @event.Type);
        var result = await handler.HandleAsync(@event, store, ct);
        if (result.IsFailure)
        {
          _logger.LogError("Can't handle webhook. {Reason}", result.Error);
          return BadRequest();
        }

        _logger.LogDebug("Event {EventType} handled successfully", @event.Type);
        return NoContent();
      }
      catch (StripeException exc)
      {
        _logger.LogError(exc, "Can't handle stripe webhook for store {StoreId}", storeId);
        return BadRequest();
      }
    }
  }
}