using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Orders.Model;
using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.Orders.Services
{
  public interface IOrderProvider
  {
    ValueTask<IPagedList<OrderRowData>> GetRowsPageAsync(Guid storeId, OrderRowsPageRequest pageRequest,
      CancellationToken ct = default);

    ValueTask<IPagedList<PurchasedProductData>> GetCustomerPurchasesPageAsync(Guid storeId,
      CustomerPurchasesPageRequest pageRequest, CancellationToken ct = default);
  }
}