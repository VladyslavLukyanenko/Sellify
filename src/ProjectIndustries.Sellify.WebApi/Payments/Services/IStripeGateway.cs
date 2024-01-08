using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services
{
  public interface IStripeGateway
  {
    ValueTask<string?> CreatePaymentSessionAsync(Guid orderId, CancellationToken ct = default);
    ValueTask CancelSessionAsync(Guid orderId, CancellationToken ct = default);
  }
}