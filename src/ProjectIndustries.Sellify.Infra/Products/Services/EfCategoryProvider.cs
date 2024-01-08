using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.App.Products.Model;
using ProjectIndustries.Sellify.App.Products.Services;
using ProjectIndustries.Sellify.Core.Products;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.Infra.Products.Services
{
  public class EfCategoryProvider : DataProvider, ICategoryProvider
  {
    private readonly IQueryable<Category> _categories;
    public EfCategoryProvider(DbContext context) : base(context)
    {
      _categories = GetAliveDataSource<Category>();
    }

    public async ValueTask<IList<CategoryGraphData>> GetCategoriesGraphAsync(Guid storeId, CancellationToken ct = default)
    {
      var dict = await _categories.Where(_ => _.StoreId == storeId)
        .Select(_ => new CategoryGraphData
        {
          Id = _.Id,
          Name = _.Name,
          Position = _.Position,
          ParentCategoryId = _.ParentCategoryId
        })
        .ToDictionaryAsync(_ => _.Id, ct);

      var result = new List<CategoryGraphData>(dict.Count);
      foreach (var category in dict.Values)
      {
        if (category.ParentCategoryId.HasValue)
        {
          var parent = dict[category.ParentCategoryId.Value];
          parent.Add(category);
        }
        else
        {
          result.Add(category);
        }
      }

      return result;
    }
  }
}