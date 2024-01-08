using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.Core.Stores;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services.PayPal
{
  public interface IPayPalWebHookHandler
  {
    bool CanHandle(string type);
    ValueTask<Result> HandleAsync(Store store, string rawPayload, CancellationToken ct = default);
  }
}