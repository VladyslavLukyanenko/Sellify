using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Products.Model;

namespace ProjectIndustries.Sellify.App.Products.Services
{
  public interface ICategoryProvider
  {
    ValueTask<IList<CategoryGraphData>> GetCategoriesGraphAsync(Guid storeId, CancellationToken ct = default);
  }
}