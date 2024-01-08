using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.App.Stores.Model;
using ProjectIndustries.Sellify.App.Stores.Services;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ControllerBase = ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers.ControllerBase;

namespace ProjectIndustries.Sellify.WebApi.Stores
{
  public class StoresController : ControllerBase
  {
    private readonly IStoreProvider _storeProvider;

    public StoresController(IStoreProvider storeProvider)
    {
      _storeProvider = storeProvider;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<StoreInfoData>))]
    public async ValueTask<IActionResult> GetStoreInfoAsync(string slug, CancellationToken ct)
    {
      var info = await _storeProvider.GetStoreInfoAsync(slug, ct);
      if (info == null)
      {
        return NotFound();
      }
      
      return Ok(info);
    }
  }
}