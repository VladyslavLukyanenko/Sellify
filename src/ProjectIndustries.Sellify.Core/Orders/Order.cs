using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using NodaTime;
using ProjectIndustries.Sellify.Core.Customers;

namespace ProjectIndustries.Sellify.Core.Orders
{
  public class Order : StoreBoundEntity<Guid>
  {
    private Order()
    {
    }

    public Order(Guid storeId, string invoiceEmail, PurchasedProduct product)
    {
      StoreId = storeId;
      Product = product;
      InvoiceEmail = invoiceEmail;
    }

    public bool IsCompleted() => Status != OrderStatus.Pending;

    public Result Fulfil(string txId, Customer customer)
    {
      if (string.IsNullOrEmpty(txId))
      {
        throw new ArgumentNullException(nameof(txId));
      }

      return EnsureNotCompleted()
        .OnSuccessTry(() =>
        {
          Status = OrderStatus.Fulfilled;
          PaidAt = SystemClock.Instance.GetCurrentInstant();
          ExternalTxId = txId;
          CustomerId = customer.Id;
        });
    }

    public Result Cancel()
    {
      return EnsureNotCompleted()
        .OnSuccessTry(() => Status = OrderStatus.Cancelled);
    }

    public void ManuallyChangeStatus(OrderStatus status)
    {
      Status = status;
    }

    private Result EnsureNotCompleted()
    {
      if (IsCompleted())
      {
        return Result.Failure("Order already completed.");
      }

      return Result.Success();
    }

    public long? CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }

    public PurchasedProduct Product { get; private set; } = new();
    public string InvoiceEmail { get; private set; } = null!;

    public IDictionary<string, string> Metadata { get; private set; } = new Dictionary<string, string>();
    public string? ExternalTxId { get; private set; } = null!;
    public Instant? PaidAt { get; private set; }
  }
}