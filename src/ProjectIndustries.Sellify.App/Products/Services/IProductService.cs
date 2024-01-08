using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.Products;

namespace ProjectIndustries.Sellify.App.Products.Services
{
  public interface IProductService
  {
    ValueTask<long> CreateAsync(Guid storeId, SaveProductCommand cmd, CancellationToken ct = default);
    ValueTask UpdateAsync(Product product, SaveProductCommand cmd, CancellationToken ct = default);
  }
}