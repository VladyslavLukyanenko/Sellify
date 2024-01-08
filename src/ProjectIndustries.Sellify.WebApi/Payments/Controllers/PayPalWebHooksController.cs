using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PayPal;
using PayPal.Api;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc;
using ProjectIndustries.Sellify.WebApi.Payments.Configs;
using ProjectIndustries.Sellify.WebApi.Payments.Services.PayPal;
using ControllerBase = ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers.ControllerBase;

namespace ProjectIndustries.Sellify.WebApi.Payments.Controllers
{
  [ApiV1HttpRoute("Payments/{storeId:guid}/PayPal/Hooks")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class PayPalWebHooksController : ControllerBase
  {
    private readonly ILogger<PayPalWebHooksController> _logger;
    private readonly PayPalGlobalConfig _config;
    private readonly IEnumerable<IPayPalWebHookHandler> _handlers;
    private readonly IStoreRepository _storeRepository;
    private readonly OAuthTokenCredential _credential;

    public PayPalWebHooksController(ILogger<PayPalWebHooksController> logger, PayPalGlobalConfig config,
      IEnumerable<IPayPalWebHookHandler> handlers, IStoreRepository storeRepository, OAuthTokenCredential credential)
    {
      _logger = logger;
      _config = config;
      _handlers = handlers;
      _storeRepository = storeRepository;
      _credential = credential;
    }

    [HttpPost]
    public async ValueTask<IActionResult> Handle(Guid storeId, CancellationToken ct)
    {
      try
      {
        _logger.LogDebug("Received paypal webhook. Searching for store with Id {StoreId}", storeId);
        var store = await _storeRepository.GetByIdAsync(storeId, ct);
        if (store == null)
        {
          _logger.LogWarning("Store {StoreId} not found", storeId);
          return NotFound();
        }

        if (store.PaymentGatewayConfigs.PayPal.IsEmpty())
        {
          return BadRequest(("PayPal integration not configured for store " + storeId).ToApiError());
        }

        _logger.LogDebug("Constructing event");
        using var reader = new StreamReader(Request.Body);
        var rawWebhookContent = await reader.ReadToEndAsync();

        var headers = new NameValueCollection(Request.Headers.Count);
        foreach (var header in Request.Headers)
        {
          headers.Add(header.Key, header.Value);
        }

        _logger.LogDebug("Obtaining access token");
        var accessToken = _credential.GetAccessToken();
        var apiContext = new APIContext(accessToken)
        {
          Config = new Dictionary<string, string>
          {
            [BaseConstants.ClientId] = _config.ClientId,
            [BaseConstants.ClientSecret] = _config.Secret,
            [BaseConstants.ApplicationModeConfig] = _config.Mode,
          }
        };

        _logger.LogDebug("Validating event");
        if (WebhookEvent.ValidateReceivedEvent(apiContext, headers, rawWebhookContent,
          store.PaymentGatewayConfigs.PayPal.WebhookId))
        {
          var @event = JsonConvert.DeserializeObject<WebhookEvent>(rawWebhookContent);
          var handler = _handlers.FirstOrDefault(_ => _.CanHandle(@event.event_type));
          if (handler != null)
          {
            _logger.LogDebug("Handling event {EventType}", @event.event_type);
            var result = await handler.HandleAsync(store, rawWebhookContent, ct);
            if (result.IsFailure)
            {
              _logger.LogError("Can't handle webhook. {Reason}", result.Error);
              return BadRequest();
            }

            _logger.LogDebug("Event {EventType} handled successfully", @event.event_type);
          }
          else
          {
            _logger.LogWarning("No handler for event {EventType}", @event.event_type);
          }

          return NoContent();
        }

        _logger.LogWarning("Event is invalid {@RawEvent}, Headers: {@Headers}", rawWebhookContent, headers);
        return BadRequest();
      }
      catch (PayPalException exc)
      {
        _logger.LogError(exc, "Can't handle PayPal webhook for store {StoreId}", storeId);
        return BadRequest();
      }
    }
  }
}