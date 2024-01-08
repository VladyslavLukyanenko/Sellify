using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Customers.Model;
using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.Customers.Services
{
  public interface ICustomerProvider
  {
    ValueTask<IPagedList<CustomerRowData>> GetCustomerRowsPageAsync(Guid storeId, CustomerRowPageRequest pageRequest,
      CancellationToken ct = default);
  }
}