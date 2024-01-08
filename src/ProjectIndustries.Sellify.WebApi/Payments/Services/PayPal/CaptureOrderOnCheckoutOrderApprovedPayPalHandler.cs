using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Newtonsoft.Json.Linq;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using ProjectIndustries.Sellify.Core.Stores;
using PayPalOrder = PayPalCheckoutSdk.Orders.Order;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services.PayPal
{
  public class CaptureOrderOnCheckoutOrderApprovedPayPalHandler : IPayPalWebHookHandler
  {
    private readonly PayPalHttpClient _payPalHttpClient;

    public CaptureOrderOnCheckoutOrderApprovedPayPalHandler(PayPalHttpClient payPalHttpClient)
    {
      _payPalHttpClient = payPalHttpClient;
    }

    public bool CanHandle(string type) => type == "CHECKOUT.ORDER.APPROVED";

    public async ValueTask<Result> HandleAsync(Store store, string rawPayload, CancellationToken ct = default)
    {
      var jobj = JObject.Parse(rawPayload);
      PayPalOrder payPalOrderFromJson = jobj["resource"]!.ToObject<PayPalOrder>()!;

      var request = new OrdersCaptureRequest(payPalOrderFromJson.Id);
      request.Prefer("return=representation");
      request.RequestBody(new OrderActionRequest());
      var response = await _payPalHttpClient.Execute(request);
      if ((int) response.StatusCode >= 400)
      {
        return Result.Failure("Can't capture order");
      }

      return Result.Success();
    }
  }
}