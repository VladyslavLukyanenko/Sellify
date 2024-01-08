using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using ProjectIndustries.Sellify.App.Analytics.Model;
using ProjectIndustries.Sellify.App.Analytics.Services;
using ProjectIndustries.Sellify.Core.Analytics.Services;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Foundation.Model;
using ProjectIndustries.Sellify.WebApi.Foundation.Mvc.Controllers;

namespace ProjectIndustries.Sellify.WebApi.Analytics.Controllers
{
  public class AnalyticsController : SecuredStoreBoundControllerBase
  {
    private readonly IAnalyticsProvider _analyticsProvider;
    private readonly IUserSessionService _userSessionService;
    private readonly IStoreRepository _storeRepository;
    private readonly ILogger<AnalyticsController> _logger;
    private readonly IMemoryCache _memoryCache;

    public AnalyticsController(IServiceProvider provider, IAnalyticsProvider analyticsProvider,
      IUserSessionService userSessionService, IStoreRepository storeRepository,
      ILogger<AnalyticsController> logger, IMemoryCache memoryCache)
      : base(provider)
    {
      _analyticsProvider = analyticsProvider;
      _userSessionService = userSessionService;
      _storeRepository = storeRepository;
      _logger = logger;
      _memoryCache = memoryCache;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<GeneralAnalytics>))]
    public async ValueTask<IActionResult> GetGeneralAsync([FromQuery] GeneralAnalyticsRequest request,
      CancellationToken ct)
    {
      var key = $"{request.Start}_{request.Offset}_{request.Period}_{CurrentUserId}_{CurrentStoreId}";
      if (_memoryCache.TryGetValue<GeneralAnalytics>(key, out var analytics))
      {
        return Ok(analytics);
      }

      var r = await _analyticsProvider.GetGeneralAnalyticsAsync(CurrentStoreId, request, ct);
      if (r.IsFailure)
      {
        return BadRequest(r.Error);
      }

      _memoryCache.Set(key, r.Value, TimeSpan.FromMinutes(1));

      return Ok(r.Value);
    }

    [HttpPost("Sessions")]
    // [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiContract<Guid>))]
    public async ValueTask<IActionResult> RefreshOrCreateSessionAsync(Guid? sessionId, CancellationToken ct)
    {
      Guid storeId;
      string? userId;
      if (User.Identity?.IsAuthenticated == false)
      {
        string referer = Request.Headers[HeaderNames.Referer];
        if (string.IsNullOrEmpty(referer) || !Uri.TryCreate(referer, UriKind.Absolute, out var refererUrl))
        {
          return BadRequest();
        }
      
        Store? store = await _storeRepository.GetByRawLocationAsync(refererUrl.AbsolutePath, ct);
        if (store == null)
        {
          return BadRequest();
        }
      
        userId = null;
        storeId = store.Id;
      }
      else
      {
        userId = CurrentUserId;
        storeId = CurrentStoreId;
      }

      string userAgent = Request.Headers[HeaderNames.UserAgent];
      var ip = HttpContext.Connection.RemoteIpAddress;
      sessionId = await _userSessionService.RefreshOrCreateSessionAsync(storeId, sessionId, userAgent, ip, userId,
        ct);

      return Ok(sessionId);
    }
  }
}