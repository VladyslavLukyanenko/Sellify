using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.Orders.Services
{
  public interface IOrderRepository
  {
    ValueTask<Order> CreateAsync(Order order, CancellationToken ct = default);
    ValueTask<Order?> GetByIdAsync(Guid orderId, CancellationToken ct = default);
    Order Update(Order order);
  }
}