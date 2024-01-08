using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.App.Products.Model;
using ProjectIndustries.Sellify.App.Products.Services;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers;

namespace ProjectIndustries.Sellify.WebApi.Products
{
  // public class FakeEntityWithPicture
  // {
  //   public string PictureId { get; set; }
  //   [ExpandPicture] public string PictureUrl { get; set; }
  //   public DayOfWeek DayOfWeek { get; set; }
  // }

  public class CategoriesController : SecuredStoreBoundControllerBase
  {
    private readonly ICategoryProvider _categoryProvider;

    public CategoriesController(IServiceProvider provider, ICategoryProvider categoryProvider) : base(provider)
    {
      _categoryProvider = categoryProvider;
    }

    // [HttpGet("Expand")]
    // [AllowAnonymous]
    // public FakeEntityWithPicture Get() => new FakeEntityWithPicture
    // {
    //   PictureId = "Test",
    //   PictureUrl = "Test",
    //   DayOfWeek = DateTime.Now.DayOfWeek
    // };

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<CategoryGraphData[]>))]
    public async ValueTask<IActionResult> GetCategoriesGraphAsync(CancellationToken ct)
    {
      var graph = await _categoryProvider.GetCategoriesGraphAsync(CurrentStoreId, ct);
      return Ok(graph);
    }
  }
}