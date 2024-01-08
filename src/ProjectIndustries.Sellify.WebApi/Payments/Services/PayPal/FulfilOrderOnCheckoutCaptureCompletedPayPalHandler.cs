using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Newtonsoft.Json.Linq;
using ProjectIndustries.Sellify.Core.Orders.Services;
using ProjectIndustries.Sellify.Core.Stores;
using PayPalOrder = PayPalCheckoutSdk.Orders.Order;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services.PayPal
{
  public class FulfilOrderOnCheckoutCaptureCompletedPayPalHandler : IPayPalWebHookHandler
  {
    private readonly IOrderManager _orderManager;

    public FulfilOrderOnCheckoutCaptureCompletedPayPalHandler(IOrderManager orderManager)
    {
      _orderManager = orderManager;
    }

    public bool CanHandle(string type) => type == "CHECKOUT.ORDER.COMPLETED";

    public async ValueTask<Result> HandleAsync(Store store, string rawPayload, CancellationToken ct = default)
    {
      var jobj = JObject.Parse(rawPayload);
      PayPalOrder payPalOrder = jobj["resource"]!.ToObject<PayPalOrder>()!;
      var purchaseUnit = payPalOrder.PurchaseUnits?.FirstOrDefault();
      if (purchaseUnit == null)
      {
        return Result.Failure("Can't get purchased unit from captured paypal order");
      }

      if (!Guid.TryParse(purchaseUnit.ReferenceId, out var orderId))
      {
        return Result.Failure("Invalid referenceId received " + purchaseUnit.ReferenceId);
      }

      var payerName = payPalOrder.Payer.Name;
      return await _orderManager.FulfilAsync(orderId, payPalOrder.Id,
        new UpdateOrCreateCustomerCommand(payPalOrder.Payer.Email, payerName?.GivenName, payerName?.Surname), ct);
    }
  }
}