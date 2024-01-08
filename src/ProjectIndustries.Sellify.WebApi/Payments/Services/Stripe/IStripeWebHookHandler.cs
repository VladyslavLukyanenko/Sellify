using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.Core.Stores;
using Stripe;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services.Stripe
{
  public interface IStripeWebHookHandler
  {
    bool CanHandle(string eventType);
    ValueTask<Result> HandleAsync(Event @event, Store store, CancellationToken ct = default);
  }
}