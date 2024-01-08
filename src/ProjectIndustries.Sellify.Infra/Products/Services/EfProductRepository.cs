using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Core.Products;
using ProjectIndustries.Sellify.Infra.Repositories;

namespace ProjectIndustries.Sellify.Infra.Products.Services
{
  public class EfProductRepository : EfSoftRemovableCrudRepository<Product, long>, IProductRepository
  {
    public EfProductRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    public async ValueTask<bool> DecrementQuantityAsync(Product product, uint quantity, CancellationToken ct = default)
    {
      var isSuccess = await DataSource
        .Where(_ => _.Id == product.Id && _.Stock >= quantity && _.Stock == product.Stock)
        .Set(_ => _.Stock, _ => _.Stock - quantity)
        .UpdateAsync(ct) == 1;
      if (isSuccess)
      {
        product.Stock -= (int) quantity;
      }

      return isSuccess;
    }
  }
}