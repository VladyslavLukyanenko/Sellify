using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Orders.Model;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services
{
  public interface IPayPalDataProvider
  {
    ValueTask<CreatePayPayOrderCommand?> GetCreatePayPayOrderCommandAsync(Guid orderId, CancellationToken ct = default);
  }
}