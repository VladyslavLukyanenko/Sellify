using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.App.Customers.Model;
using ProjectIndustries.Sellify.App.Customers.Services;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.Core.Customers;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.Infra.Customers.Services
{
  public class EfCustomerProvider : DataProvider, ICustomerProvider
  {
    private readonly IQueryable<Customer> _aliveCustomers;
    private readonly IQueryable<Order> _orders;

    public EfCustomerProvider(DbContext context) : base(context)
    {
      _aliveCustomers = GetAliveDataSource<Customer>();
      _orders = GetDataSource<Order>();
    }

    public async ValueTask<IPagedList<CustomerRowData>> GetCustomerRowsPageAsync(Guid storeId,
      CustomerRowPageRequest pageRequest, CancellationToken ct = default)
    {
      var orderDetails = _orders.Where(_ => _.StoreId == storeId)
        .Select(o => new
        {
          o.Id,
          o.PaidAt,
          Price = o.Product.Price * o.Product.Quantity,
          o.CustomerId
        })
        .GroupBy(_ => _.CustomerId)
        .Select(_ => new
        {
          CustomerId = _.Key,
          TotalOrdersCount = _.Count(),
          FirstPuchaseAt = _.Min(q => q.PaidAt),
          LastPurchasedAt = _.Max(q => q.PaidAt),
          TotalSpent = _.Sum(q => q.Price)
        });

      var query = from c in _aliveCustomers
        join o in orderDetails on c.Id equals o.CustomerId
        where c.StoreId == storeId
        select new {c, o};

      if (!pageRequest.IsSearchTermEmpty())
      {
        var term = pageRequest.SearchTerm!.ToUpper();

        query = query.Where(_ => _.c.Email.ToUpper().Contains(term)
                                 || _.c.FirstName != null && _.c.FirstName.ToUpper().Contains(term)
                                 || _.c.LastName != null && _.c.LastName.ToUpper().Contains(term)
                                 || _orders.Any(q =>
                                   q.CustomerId == _.c.Id && q.Product.Title.ToUpper().Contains(term)));
      }

      if (pageRequest.IsFilteredByTimePeriod())
      {
        query = query.Where(_ => _.o.FirstPuchaseAt >= pageRequest.StartAt && _.o.FirstPuchaseAt <= pageRequest.EndAt
                                 || _.o.LastPurchasedAt >= pageRequest.StartAt
                                 && _.o.LastPurchasedAt <= pageRequest.EndAt);
      }

      return await query.Select(x => new CustomerRowData
        {
          Id = x.c.Id,
          Email = x.c.Email,
          LastName = x.c.LastName,
          FirstName = x.c.FirstName,
          TotalSpent = x.o.TotalSpent,
          TotalOrdersCount = x.o.TotalOrdersCount,
          LastPurchasedAt = x.o.LastPurchasedAt
        })
        .PaginateAsync(pageRequest, ct);
    }
  }
}