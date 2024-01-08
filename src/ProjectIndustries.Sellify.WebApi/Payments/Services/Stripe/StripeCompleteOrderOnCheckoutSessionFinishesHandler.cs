using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectIndustries.Sellify.App.Orders.Services;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Core.Orders.Services;
using ProjectIndustries.Sellify.Core.Stores;
using Stripe;
using Stripe.Checkout;
using Order = ProjectIndustries.Sellify.Core.Orders.Order;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services.Stripe
{
  public class StripeCompleteOrderOnCheckoutSessionFinishesHandler : IStripeWebHookHandler
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderManager _orderManager;
    private readonly ILogger<StripeCompleteOrderOnCheckoutSessionFinishesHandler> _logger;
    private readonly IStripeClientFactory _stripeClientFactory;

    public StripeCompleteOrderOnCheckoutSessionFinishesHandler(IOrderRepository orderRepository,
      ILogger<StripeCompleteOrderOnCheckoutSessionFinishesHandler> logger, IOrderManager orderManager,
      IStripeClientFactory stripeClientFactory)
    {
      _orderRepository = orderRepository;
      _logger = logger;
      _orderManager = orderManager;
      _stripeClientFactory = stripeClientFactory;
    }

    public bool CanHandle(string eventType) => eventType is Events.CheckoutSessionCompleted;

    public async ValueTask<Result> HandleAsync(Event @event, Store store, CancellationToken ct = default)
    {
      var session = (Session) @event.Data.Object;
      var orderId = Guid.Parse(session.Metadata["orderId"]);
      var order = await _orderRepository.GetByIdAsync(orderId, ct);
      _logger.LogDebug("Processing charge {Charge} for order {OrderId}", session.Id, orderId);

      if (order == null)
      {
        return Result.Failure($"Order {orderId} not found");
      }

      if (order.IsCompleted())
      {
        return Result.Failure($"Order {orderId} already completed");
      }

      switch (@event.Type)
      {
        case Events.CheckoutSessionCompleted:
          if (session.PaymentStatus == "paid")
          {
            await FulfillOrder(store, order, session, ct);
          }

          break;
        case Events.CheckoutSessionAsyncPaymentSucceeded:
          await FulfillOrder(store, order, session, ct);
          break;
        case Events.CheckoutSessionAsyncPaymentFailed:
          order.Cancel();
          _logger.LogDebug("Order {OrderId} cancelled", orderId);
          break;
      }

      _orderRepository.Update(order);

      return Result.Success();
    }

    private async ValueTask<Result> FulfillOrder(Store store, Order order, Session session, CancellationToken ct)
    {
      var customerService = new CustomerService(_stripeClientFactory.CreateClient(store));
      Customer customer = await customerService.GetAsync(session.CustomerId, cancellationToken: ct);
      var result = await _orderManager.FulfilAsync(order, session.Id,
        new UpdateOrCreateCustomerCommand(customer.Email, customer.Name, null), ct);
      if (result.IsSuccess)
      {
        _logger.LogDebug("Order {OrderId} fulfilled", order.Id);
      }

      return result;
    }
  }
}