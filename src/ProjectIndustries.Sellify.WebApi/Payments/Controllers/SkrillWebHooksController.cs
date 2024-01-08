using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc;
using ProjectIndustries.Sellify.WebApi.Payments.Data;
using ProjectIndustries.Sellify.WebApi.Payments.Services;
using ProjectIndustries.Sellify.WebApi.Payments.Services.Skrill;
using ControllerBase = ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers.ControllerBase;

namespace ProjectIndustries.Sellify.WebApi.Payments.Controllers
{
  [ApiV1HttpRoute("Payments/{storeId:guid}/Skrill/Hooks")]
  [ApiExplorerSettings(IgnoreApi = true)]
  public class SkrillWebHooksController : ControllerBase
  {
    private readonly ILogger<SkrillWebHooksController> _logger;
    private readonly ISkrillWebhookValidator _skrillWebhookValidator;
    private readonly IStoreRepository _storeRepository;
    private readonly IEnumerable<ISkrillWebHookHandler> _handlers;

    public SkrillWebHooksController(ILogger<SkrillWebHooksController> logger,
      ISkrillWebhookValidator skrillWebhookValidator, IStoreRepository storeRepository,
      IEnumerable<ISkrillWebHookHandler> handlers)
    {
      _logger = logger;
      _skrillWebhookValidator = skrillWebhookValidator;
      _storeRepository = storeRepository;
      _handlers = handlers;
    }

    [HttpPost(Name = "SkillWebhookHandler")]
    public async ValueTask<IActionResult> Handle(Guid storeId , [FromForm] SkrillWebhookData data,
      CancellationToken ct)
    {
      _logger.LogDebug("Received skrill webhook. Searching for store with Id {StoreId}", storeId);
      var store = await _storeRepository.GetByIdAsync(storeId, ct);
      if (store == null)
      {
        _logger.LogWarning("Store {StoreId} not found", storeId);
        return NotFound();
      }

      if (store.PaymentGatewayConfigs.Skrill.IsEmpty())
      {
        return BadRequest(("Skrill integration not configured for store " + storeId).ToApiError());
      }

      var isValidResult = _skrillWebhookValidator.IsValid(store.PaymentGatewayConfigs.Skrill, data);
      if (isValidResult.IsFailure || !isValidResult.Value)
      {
        _logger.LogWarning("Event is invalid {@Event}, Details: {@Details}", data, isValidResult.Error);
        return BadRequest();
      }

      _logger.LogDebug("Handling event  with status {ProcessingStatus}, TransactionId {TransactionId}", data.Status,
        data.TransactionId);
      var handler = _handlers.FirstOrDefault(_ => _.CanHandle(data.Status));
      if (handler == null)
      {
        _logger.LogWarning("No handler for status {ProcessingStatus}", data.Status);
        return NoContent();
      }

      try
      {
        var result = await handler.HandleAsync(store, data, ct);
        if (result.IsFailure)
        {
          _logger.LogError("Can't handle webhook. {Reason}", result.Error);
          return BadRequest();
        }

        _logger.LogDebug("Event with status {ProcessingStatus}, TransactionId {TransactionId} handled successfully",
          data.Status, data.TransactionId);
        return NoContent();
      }
      catch (Exception exc)
      {
        _logger.LogError(exc, "Can't handle Skrill webhook for store {StoreId}", storeId);
        return BadRequest();
      }
    }
  }
}