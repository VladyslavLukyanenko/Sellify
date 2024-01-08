using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.App.Orders.Model;
using ProjectIndustries.Sellify.App.Orders.Services;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.Core.Customers;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.Infra.Orders.Services
{
  public class EfOrderProvider : DataProvider, IOrderProvider
  {
    private readonly IQueryable<Order> _aliveOrders;
    private readonly IQueryable<Customer> _customers;

    public EfOrderProvider(DbContext context) : base(context)
    {
      _aliveOrders = GetAliveDataSource<Order>();
      _customers = GetDataSource<Customer>();
    }

    public async ValueTask<IPagedList<OrderRowData>> GetRowsPageAsync(Guid storeId, OrderRowsPageRequest pageRequest,
      CancellationToken ct = default)
    {
      var query = from o in _aliveOrders
        join c in _customers on o.CustomerId equals c.Id into tmp
        from c in tmp.DefaultIfEmpty()
        where o.StoreId == storeId
        select new OrderRowData
        {
          // Currency = tx.Currency.Id,
          InvoiceEmail = o.InvoiceEmail,
          CustomerEmail = c == null ? null : c.Email,
          CustomerName = c == null || c.FirstName == null ? null : c.FirstName + " " + c.LastName,
          Id = o.Id,
          TotalPrice = o.Product.Quantity * o.Product.Price,
          Status = o.Status,
          CreatedAt = o.CreatedAt,
          PurchasedProduct = new PurchasedProductData
          {
            Price = o.Product.Price,
            OrderId = o.Id,
            Quantity = o.Product.Quantity,
            OrderStatus = o.Status,
            ProductId = o.Product.Id,
            Title = o.Product.Title,
            PaidAt = o.PaidAt,
          }
        };

      if (!pageRequest.IsSearchTermEmpty())
      {
        var q = pageRequest.SearchTerm!.ToUpperInvariant();
        query = query.Where(_ => _.CustomerEmail != null && _.CustomerEmail.ToUpper().Contains(q)
                                 || _.CustomerName != null && _.CustomerName.ToUpper().Contains(q)
                                 || _.InvoiceEmail.ToUpper().Contains(q)
                                 || _.PurchasedProduct.Title.ToUpper().Contains(q));
      }

      if (pageRequest.ExpectedStatus.HasValue)
      {
        query = query.Where(_ => _.Status == pageRequest.ExpectedStatus);
      }

      if (pageRequest.IsFilteredByTimePeriod())
      {
        query = query.Where(_ => _.CreatedAt >= pageRequest.StartAt && _.CreatedAt <= pageRequest.EndAt);
      }

      return await query
        .OrderByDescending(_ => _.CreatedAt)
        .PaginateAsync(pageRequest, ct);
    }

    public async ValueTask<IPagedList<PurchasedProductData>> GetCustomerPurchasesPageAsync(Guid storeId,
      CustomerPurchasesPageRequest pageRequest, CancellationToken ct = default)
    {
      var query = from o in _aliveOrders
        where o.StoreId == storeId && o.CustomerId == pageRequest.CustomerId
        orderby o.CreatedAt descending
        select new PurchasedProductData
        {
          Price = o.Product.Price,
          Quantity = o.Product.Quantity,
          OrderId = o.Id,
          OrderStatus = o.Status,
          ProductId = o.Product.Id,
          Title = o.Product.Title,
          PaidAt = o.PaidAt,
        };

      return await query.PaginateAsync(pageRequest, ct);
    }
  }
}