using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.Products
{
  public interface IProductRepository
  {
    ValueTask<Product?> GetByIdAsync(long id, CancellationToken ct = default);
    void Remove(Product product);
    ValueTask<Product> CreateAsync(Product product, CancellationToken ct = default);
    Product Update(Product product);
    ValueTask<bool> DecrementQuantityAsync(Product product, uint quantity, CancellationToken ct = default);
  }
}