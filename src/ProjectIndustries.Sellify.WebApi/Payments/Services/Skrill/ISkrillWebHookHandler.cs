using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Payments.Data;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services.Skrill
{
  public interface ISkrillWebHookHandler
  {
    bool CanHandle(SkrillStatus status);
    ValueTask<Result> HandleAsync(Store store, SkrillWebhookData data, CancellationToken ct = default);
  }
}