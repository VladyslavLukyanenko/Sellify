using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.App.Products.Model;
using ProjectIndustries.Sellify.App.Products.Services;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.Core.Products;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers;

namespace ProjectIndustries.Sellify.WebApi.Products
{
  public class ProductsController : SecuredStoreBoundControllerBase
  {
    private readonly IProductProvider _productProvider;
    private readonly IProductRepository _productRepository;
    private readonly IProductService _productService;

    public ProductsController(IServiceProvider provider, IProductProvider productProvider,
      IProductRepository productRepository, IProductService productService)
      : base(provider)
    {
      _productProvider = productProvider;
      _productRepository = productRepository;
      _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<IPagedList<ProductRowData>>))]
    public async ValueTask<IActionResult> GetPage([FromQuery] ProductPageRequest pageRequest, CancellationToken ct)
    {
      var page = await _productProvider.GetProductsPageAsync(CurrentStoreId, pageRequest, ct);
      return Ok(page);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<ProductData>))]
    public async ValueTask<IActionResult> GetByIdAsync(long id, CancellationToken ct)
    {
      return Ok(await _productProvider.GetByIdAsync(id, ct));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask CreateAsync([FromBody] SaveProductCommand cmd, CancellationToken ct)
    {
      await _productService.CreateAsync(CurrentStoreId, cmd, ct);
    }


    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> UpdateAsync(long id, [FromBody] SaveProductCommand cmd, CancellationToken ct)
    {
      Product? product = await _productRepository.GetByIdAsync(id, ct);
      if (product == null)
      {
        return NotFound();
      }

      await _productService.UpdateAsync(product, cmd, ct);

      return NoContent();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> RemoveAsync(long id, CancellationToken ct)
    {
      Product? product = await _productRepository.GetByIdAsync(id, ct);
      if (product == null)
      {
        return NotFound();
      }

      _productRepository.Remove(product);
      return NoContent();
    }
  }
}