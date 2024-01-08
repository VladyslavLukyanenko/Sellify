using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.Stores;
using Stripe;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services
{
  public interface IStripeClientFactory
  {
    ValueTask<IStripeClient> CreateClientAsync(Guid storeId, CancellationToken ct = default);
    IStripeClient CreateClient(Store store);
  }
}