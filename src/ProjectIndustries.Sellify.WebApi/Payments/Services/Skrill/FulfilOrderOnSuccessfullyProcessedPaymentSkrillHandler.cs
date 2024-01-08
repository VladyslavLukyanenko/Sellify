using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.App.Orders.Services;
using ProjectIndustries.Sellify.Core.Orders.Services;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Payments.Data;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services.Skrill
{
  public class FulfilOrderOnSuccessfullyProcessedPaymentSkrillHandler : ISkrillWebHookHandler
  {
    private readonly IOrderManager _orderManager;

    public FulfilOrderOnSuccessfullyProcessedPaymentSkrillHandler(IOrderManager orderManager)
    {
      _orderManager = orderManager;
    }

    public bool CanHandle(SkrillStatus status) => status == SkrillStatus.Processed;

    public async ValueTask<Result> HandleAsync(Store store, SkrillWebhookData data, CancellationToken ct = default)
    {
      return await _orderManager.FulfilAsync(data.OrderId, data.TransactionId,
        new UpdateOrCreateCustomerCommand(data.Email, data.FirstName, data.LastName), ct);
    }
  }
}