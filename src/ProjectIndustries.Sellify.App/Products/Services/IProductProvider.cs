using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Products.Model;
using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.Products.Services
{
  public interface IProductProvider
  {
    ValueTask<IPagedList<ProductRowData>> GetProductsPageAsync(Guid storeId, ProductPageRequest request,
      CancellationToken ct = default);

    ValueTask<ProductData> GetByIdAsync(long productId, CancellationToken ct = default);
  }
}