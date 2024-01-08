using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace ProjectIndustries.Sellify.Core.Orders.Services
{
  public interface IOrderManager
  {
    ValueTask<Result<Order?>> CreateAsync(CreateOrderCommand cmd, CancellationToken ct = default);

    ValueTask<Result> FulfilAsync(Guid orderId, string txId, UpdateOrCreateCustomerCommand cmd,
      CancellationToken ct = default);

    ValueTask<Result> FulfilAsync(Order order, string txId, UpdateOrCreateCustomerCommand cmd,
      CancellationToken ct = default);
  }
}