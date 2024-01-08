using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectIndustries.Sellify.App.Model;
using ProjectIndustries.Sellify.App.WebHooks.Data;
using ProjectIndustries.Sellify.App.WebHooks.Services;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers;

namespace ProjectIndustries.Sellify.WebApi.Webhooks.Controllers
{
  public class WebhooksController : SecuredStoreBoundControllerBase
  {
    private readonly IWebhooksProvider _webhooksProvider;
    private readonly IWebHookBindingService _webHookBindingService;
    private readonly IWebHookBindingRepository _webHookBindingRepository;

    public WebhooksController(IServiceProvider provider, IWebhooksProvider webhooksProvider,
      IWebHookBindingService webHookBindingService, IWebHookBindingRepository webHookBindingRepository)
      : base(provider)
    {
      _webhooksProvider = webhooksProvider;
      _webHookBindingService = webHookBindingService;
      _webHookBindingRepository = webHookBindingRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<IPagedList<WebhookRowData>>))]
    public async ValueTask<IActionResult> GetPageAsync([FromQuery] FilteredPageRequest request, CancellationToken ct)
    {
      var page = await _webhooksProvider.GetBindingRowsPageAsync(CurrentStoreId, request, ct);
      return Ok(page);
    }

    [HttpGet("Supported")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<string[]>))]
    public IActionResult GetSupportedWebhooks()
    {
      return Ok(_webhooksProvider.GetSupportedWebhooks());
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask CreateAsync([FromBody] SaveBindingCommand cmd, CancellationToken ct)
    {
      await _webHookBindingService.CreateAsync(CurrentStoreId, cmd, ct);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> UpdateAsync(long id, [FromBody] SaveBindingCommand cmd, CancellationToken ct)
    {
      WebHookBinding? binding = await _webHookBindingRepository.GetByIdAsync(id, ct);
      if (binding == null)
      {
        return NotFound();
      }

      await _webHookBindingService.UpdateAsync(binding, cmd, ct);
      return NoContent();
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async ValueTask<IActionResult> RemoveAsync(long id, CancellationToken ct)
    {
      WebHookBinding? binding = await _webHookBindingRepository.GetByIdAsync(id, ct);
      if (binding == null)
      {
        return NotFound();
      }

      _webHookBindingRepository.Remove(binding);
      return NoContent();
    }
  }
}