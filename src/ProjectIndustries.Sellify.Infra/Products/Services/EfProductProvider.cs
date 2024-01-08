using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.App.Products.Model;
using ProjectIndustries.Sellify.App.Products.Services;
using ProjectIndustries.Sellify.App.Services;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.Core.Products;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.Infra.Products.Services
{
  public class EfProductProvider : DataProvider, IProductProvider
  {
    private readonly IQueryable<Product> _products;
    private readonly IQueryable<Category> _categories;
    private readonly IMapper _mapper;
    private readonly IPathsService _pathsService;

    public EfProductProvider(DbContext context, IMapper mapper, IPathsService pathsService)
      : base(context)
    {
      _mapper = mapper;
      _pathsService = pathsService;
      _products = GetAliveDataSource<Product>();
      _categories = GetAliveDataSource<Category>();
    }

    public async ValueTask<IPagedList<ProductRowData>> GetProductsPageAsync(Guid storeId, ProductPageRequest request,
      CancellationToken ct = default)
    {
      var query = from p in _products
        join c in _categories on p.CategoryId equals c.Id
        where p.StoreId == storeId
        select new {p, c};

      if (!request.IsSearchTermEmpty())
      {
        var term = request.SearchTerm!.ToUpper();
        query = query.Where(_ => _.p.Title.ToUpper().Contains(term)
                                 || _.p.Excerpt.ToUpper().Contains(term)
                                 || _.c.Name.ToUpper().Contains(term));
      }

      var page = await query.OrderByDescending(_ => _.p.CreatedAt)
        .ThenBy(_ => _.p.Title)
        .Select(_ => new ProductRowData
        {
          Id = _.p.Id,
          Title = _.p.Title,
          Excerpt = _.p.Excerpt,
          Picture = _.p.Picture,
          Category = _.c.Name,
          Stock = _.p.Stock,
          Price = _.p.Price
        })
        .PaginateAsync(request, ct);

      foreach (var data in page.Content)
      {
        data.Picture = _pathsService.ToAbsoluteUrl(data.Picture);
      }

      return page;
    }

    public async ValueTask<ProductData> GetByIdAsync(long productId, CancellationToken ct = default)
    {
      var data = await _products.Where(_ => _.Id == productId)
        .ProjectTo<ProductData>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(ct);

      data.Picture = _pathsService.ToAbsoluteUrl(data.Picture);

      return data;
    }
  }
}