using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core;
using ProjectIndustries.Sellify.Core.Stores;
using Stripe;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services
{
  public class StripeClientFactory : IStripeClientFactory
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IStoreRepository _storeRepository;

    public StripeClientFactory(IHttpClientFactory httpClientFactory, IStoreRepository storeRepository)
    {
      _httpClientFactory = httpClientFactory;
      _storeRepository = storeRepository;
    }

    public async ValueTask<IStripeClient> CreateClientAsync(Guid storeId, CancellationToken ct = default)
    {
      var store = await _storeRepository.GetByIdAsync(storeId, ct)
                      ?? throw new CoreException("Can't find store with id " + storeId);
      return CreateClient(store);
    }

    public IStripeClient CreateClient(Store store)
    {
      var client = _httpClientFactory.CreateClient();
      return new StripeClient(httpClient: new SystemNetHttpClient(client),
        apiKey: store.PaymentGatewayConfigs.Stripe.ApiKey);
    }
  }
}