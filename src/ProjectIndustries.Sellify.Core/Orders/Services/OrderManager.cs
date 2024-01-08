using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.Core.Customers;
using ProjectIndustries.Sellify.Core.Products;

namespace ProjectIndustries.Sellify.Core.Orders.Services
{
  public class OrderManager : IOrderManager
  {
    readonly ICustomerRepository _customerRepository;
    readonly IOrderRepository _orderRepository;
    readonly IProductRepository _productRepository;

    public OrderManager(ICustomerRepository customerRepository, IOrderRepository orderRepository,
      IProductRepository productRepository)
    {
      _customerRepository = customerRepository;
      _orderRepository = orderRepository;
      _productRepository = productRepository;
    }

    public async ValueTask<Result<Order?>> CreateAsync(CreateOrderCommand cmd,
      CancellationToken ct = default)
    {
      Product? product = await _productRepository.GetByIdAsync(cmd.ProductId, ct);
      if (product == null)
      {
        return null;
      }

      if (!await _productRepository.DecrementQuantityAsync(product, cmd.Quantity, ct))
      {
        return Result.Failure<Order?>("Can't decrement product stock count");
      }

      var purchasedProduct = new PurchasedProduct(product, cmd.Quantity);
      return await _orderRepository.CreateAsync(new Order(product.StoreId, cmd.CustomerEmail, purchasedProduct), ct);
    }

    public async ValueTask<Result> FulfilAsync(Guid orderId, string txId, UpdateOrCreateCustomerCommand cmd,
      CancellationToken ct = default)
    {
      var order = await _orderRepository.GetByIdAsync(orderId, ct);
      if (order == null)
      {
        return Result.Failure($"Order {orderId} not found");
      }

      return await FulfilAsync(order, txId, cmd, ct);
    }

    public async ValueTask<Result> FulfilAsync(Order order, string txId, UpdateOrCreateCustomerCommand cmd,
      CancellationToken ct = default)
    {
      if (order.IsCompleted())
      {
        return Result.Failure($"Order {order.Id} already completed");
      }

      Customer? customer = await _customerRepository.GetByEmailAsync(order.StoreId, cmd.CustomerEmail, ct);
      if (customer == null)
      {
        customer = new Customer(order.StoreId, cmd.CustomerEmail);
        customer = await _customerRepository.CreateAsync(customer, ct);
      }

      if (!string.IsNullOrEmpty(cmd.FirstName))
      {
        customer.FirstName = cmd.FirstName;
        customer.LastName = cmd.LastName;

        _customerRepository.Update(customer);
      }

      order.Fulfil(txId, customer);
      return Result.Success();
    }
  }
}