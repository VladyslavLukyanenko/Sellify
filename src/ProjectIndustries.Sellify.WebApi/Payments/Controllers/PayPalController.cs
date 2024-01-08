using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using ProjectIndustries.Sellify.App.Orders.Model;
using ProjectIndustries.Sellify.Core.Orders.Services;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc;
using ProjectIndustries.Sellify.WebApi.Payments.Services;
using ControllerBase = ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers.ControllerBase;
using PayPalOrder = PayPalCheckoutSdk.Orders.Order;

namespace ProjectIndustries.Sellify.WebApi.Payments.Controllers
{
  [ApiV1HttpRoute("Payments/[controller]")]
  public class PayPalController : ControllerBase
  {
    private readonly PayPalHttpClient _payPalHttpClient;
    private readonly IOrderRepository _orderRepository;
    private readonly IPayPalDataProvider _payPalDataProvider;
    private readonly IOrderManager _orderManager;

    public PayPalController(PayPalHttpClient payPalHttpClient, IOrderRepository orderRepository,
      IPayPalDataProvider payPalDataProvider, IOrderManager orderManager)
    {
      _payPalHttpClient = payPalHttpClient;
      _orderRepository = orderRepository;
      _payPalDataProvider = payPalDataProvider;
      _orderManager = orderManager;
    }

    // create order
    [HttpPost("{orderId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<string>))]
    public async ValueTask<IActionResult> CreatePayPalOrder(Guid orderId, CancellationToken ct)
    {
      var cmd = await _payPalDataProvider.GetCreatePayPayOrderCommandAsync(orderId, ct);
      if (cmd == null)
      {
        return NotFound();
      }

      var request = new OrdersCreateRequest();
      request.Prefer("return=representation");

      request.RequestBody(BuildRequestBody(cmd));

      var response = await _payPalHttpClient.Execute(request);
      PayPalOrder payPalOrder = response.Result<PayPalOrder>();

      return Ok(payPalOrder.Id);
    }

    [HttpPost("{payPalOrderId}/Capture")]
    public async ValueTask<IActionResult> Capture(string payPalOrderId, CancellationToken ct)
    {
      var request = new OrdersCaptureRequest(payPalOrderId);
      request.Prefer("return=representation");
      request.RequestBody(new OrderActionRequest());
      var response = await _payPalHttpClient.Execute(request);
      if ((int) response.StatusCode >= 400)
      {
        return BadRequest();
      }

      var payPalOrder = response.Result<PayPalOrder>();
      var purchaseUnit = payPalOrder.PurchaseUnits.FirstOrDefault();
      if (purchaseUnit == null)
      {
        return BadRequest();
      }

      if (!Guid.TryParse(purchaseUnit.ReferenceId, out var orderId))
      {
        return BadRequest();
      }

      var payerName = payPalOrder.Payer.Name;
      var fulfilResult = await _orderManager.FulfilAsync(orderId, payPalOrderId,
        new UpdateOrCreateCustomerCommand(payPalOrder.Payer.Email, payerName?.GivenName, payerName?.Surname), ct);

      if (fulfilResult.IsFailure)
      {
        return BadRequest(fulfilResult.Error.ToApiError());
      }

      return NoContent();
    }


    private static OrderRequest BuildRequestBody(CreatePayPayOrderCommand data)
    {
      OrderRequest orderRequest = new()
      {
        CheckoutPaymentIntent = "CAPTURE",
        Payer = new Payer
        {
          Email = data.PayerEmail
        },
        ApplicationContext = new ApplicationContext
        {
          BrandName = "Sellify",
          LandingPage = "BILLING",
          UserAction = "PAY_NOW",
          ShippingPreference = "NO_SHIPPING"
        },
        PurchaseUnits = new List<PurchaseUnitRequest>
        {
          new()
          {
            ReferenceId = data.OrderId,
            Description = data.ProductDesc,
            Payee = new Payee
            {
              Email = data.PayeeEmail,
              PayeeDisplayable = new PayeeDisplayable
              {
                BrandName = data.StoreTitle + " | Sellify"
              }
            },
            // SoftDescriptor = product.,
            AmountWithBreakdown = new AmountWithBreakdown
            {
              CurrencyCode = "USD",
              Value = data.TotalPrice,
              AmountBreakdown = new AmountBreakdown
              {
                ItemTotal = new Money
                {
                  CurrencyCode = "USD",
                  Value = data.TotalPrice
                }
              }
            },
            Items = new List<Item>
            {
              new()
              {
                Name = data.ProductTitle,
                Description = data.ProductDesc,
                Sku = data.ProductSKU,
                UnitAmount = new Money
                {
                  CurrencyCode = "USD",
                  Value = data.PricePerItem
                },
                Quantity = data.Quantity,
                // Category = data.CategoryName
              }
            }
          }
        }
      };

      return orderRequest;
    }
  }
}