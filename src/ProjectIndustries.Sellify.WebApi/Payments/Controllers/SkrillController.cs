using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Core.Orders.Services;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc;
using ProjectIndustries.Sellify.WebApi.Payments.Configs;
using ControllerBase = ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers.ControllerBase;

namespace ProjectIndustries.Sellify.WebApi.Payments.Controllers
{
  [ApiV1HttpRoute("Payments/[controller]")]
  public class SkrillController : ControllerBase
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStoreRepository _storeRepository;
    private readonly SkrillGlobalConfig _config;

    public SkrillController(IOrderRepository orderRepository, IHttpClientFactory httpClientFactory,
      IStoreRepository storeRepository, SkrillGlobalConfig config)
    {
      _orderRepository = orderRepository;
      _httpClientFactory = httpClientFactory;
      _storeRepository = storeRepository;
      _config = config;
    }

    [HttpPost("{orderId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<string>))]
    public async ValueTask<IActionResult> CreateSession(Guid orderId, CancellationToken ct)
    {
      Order? order = await _orderRepository.GetByIdAsync(orderId, ct);
      if (order == null)
      {
        return NotFound();
      }

      Store store = (await _storeRepository.GetByIdAsync(order.StoreId, ct))!;
      var httpClient = _httpClientFactory.CreateClient(KnownHttpClients.Skrill);
      var webhookHandlerUrl = Url.RouteUrl("SkillWebhookHandler", new {storeId = store.Id}, Request.Scheme,
        /*Request.Host.Value*/ "3002d14d2bea.ngrok.io");
      var data = new Dictionary<string, string?>
      {
        ["pay_to_email"] = store.PaymentGatewayConfigs.Skrill.Email,
        ["pay_from_email"] = order.InvoiceEmail,
        ["return_url_target"] = "1",
        ["cancel_url_target"] = "1",
        ["status_url"] = webhookHandlerUrl,
        ["language"] = "EN",
        ["merchant_fields"] = "orderId",
        ["orderId"] = order.Id.ToString(),
        ["amount"] = order.Product.CalculateTotalPrice().ToString(CultureInfo.InvariantCulture),
        ["currency"] = "USD",
        ["detail1_description"] = "Product:",
        ["detail1_text"] = order.Product.Title,
        ["ondemand_max_currency"] = "USD",
        ["prepare_only"] = "1",
        ["submit_id"] = "Submit",
        ["Pay"] = "Pay",
      };

      HttpContent payload = new FormUrlEncodedContent(data!);

      var response = await httpClient.PostAsync(_config.CreateSessionEndpoint, payload, ct);
      var responseContent = await response.Content.ReadAsStringAsync(ct);
      if (!response.IsSuccessStatusCode)
      {
        return BadRequest(responseContent);
      }

      var checkoutUrl = _config.BuildQuickCheckoutUrl(responseContent);
      return Ok(checkoutUrl);
    }
  }

  public static class KnownHttpClients
  {
    public const string Skrill = nameof(Skrill);
  }
}