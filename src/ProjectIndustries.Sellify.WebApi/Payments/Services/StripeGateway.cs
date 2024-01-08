using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Services;
using ProjectIndustries.Sellify.Core.Orders.Services;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Payments.Configs;
using Stripe;
using Stripe.Checkout;
using Order = ProjectIndustries.Sellify.Core.Orders.Order;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services
{
  public class StripeGateway : IStripeGateway
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IPathsService _pathsService;
    private readonly IStripeClientFactory _stripeClientFactory;
    private readonly StripeGlobalConfig _stripeConfig;
    private readonly IStoreRepository _storeRepository;

    public StripeGateway(IOrderRepository orderRepository, IPathsService pathsService,
      IStripeClientFactory stripeClientFactory, StripeGlobalConfig stripeConfig, IStoreRepository storeRepository)
    {
      _orderRepository = orderRepository;
      _pathsService = pathsService;
      _stripeClientFactory = stripeClientFactory;
      _stripeConfig = stripeConfig;
      _storeRepository = storeRepository;
    }

    public async ValueTask<string?> CreatePaymentSessionAsync(Guid orderId, CancellationToken ct = default)
    {
      Order? order = await _orderRepository.GetByIdAsync(orderId, ct);
      if (order == null)
      {
        return null;
      }

      Store store = (await _storeRepository.GetByIdAsync(order.StoreId, ct))!;

      var sessionService = new SessionService(_stripeClientFactory.CreateClient(store));
      var images = new List<string>();
      var pic = _pathsService.ToAbsoluteUrl(order.Product.Picture);
      if (!string.IsNullOrEmpty(pic))
      {
        images.Add(pic);
      }

      var session = await sessionService.CreateAsync(new SessionCreateOptions
      {
        PaymentMethodTypes = new List<string>
        {
          "card"
        },
        CustomerEmail = order.InvoiceEmail,
        SuccessUrl = _stripeConfig.GetPaymentSuccessfulUrl(store),
        CancelUrl = _stripeConfig.GetPaymentCancelledUrl(store),
        LineItems = new List<SessionLineItemOptions>
        {
          new()
          {
            Amount = (long) order.Product.Price * 100L * order.Product.Quantity,
            Currency = "USD",
            // Description = order.Product.,
            Images = images,
            Name = order.Product.Title,
            Quantity = order.Product.Quantity
          }
        },
        PaymentIntentData = new SessionPaymentIntentDataOptions
        {
          // CaptureMethod = "manual",
          Metadata = new Dictionary<string, string>
          {
            {"orderId", order.Id.ToString()},
          }
        },
        Metadata = new Dictionary<string, string>
        {
          {"orderId", order.Id.ToString()},
        }
      }, cancellationToken: ct);

      order.Metadata["sessionId"] = session.Id;
      order.Metadata["paymentIntentId"] = session.PaymentIntentId;

      return session.Id;
    }

    public async ValueTask CancelSessionAsync(Guid orderId, CancellationToken ct = default)
    {
      Order? order = await _orderRepository.GetByIdAsync(orderId, ct);
      if (order == null)
      {
        return;
      }

      Store store = (await _storeRepository.GetByIdAsync(order.StoreId, ct))!;

      var paymentIntentId = order.Metadata["paymentIntentId"];
      var paymentIntentService = new PaymentIntentService(_stripeClientFactory.CreateClient(store));
      await paymentIntentService.CancelAsync(paymentIntentId, cancellationToken: ct);
      order.Cancel();
      _orderRepository.Update(order);
    }
  }
}