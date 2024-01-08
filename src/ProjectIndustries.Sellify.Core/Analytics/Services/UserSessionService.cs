using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.Analytics.Services
{
  public class UserSessionService : IUserSessionService
  {
    private static ConcurrentDictionary<string, SemaphoreSlim> Gates = new();
    private readonly IUserSessionRepository _userSessionRepository;

    public UserSessionService(IUserSessionRepository userSessionRepository)
    {
      _userSessionRepository = userSessionRepository;
    }

    public async ValueTask<Guid> RefreshOrCreateSessionAsync(Guid storeId, Guid? sessionId, string userAgent,
      IPAddress? ipAddress, string? userId, CancellationToken ct = default)
    {
      var gatesKey = userId ?? sessionId?.ToString();
      var gates = gatesKey != null ? Gates.GetOrAdd(gatesKey, _ => new SemaphoreSlim(1, 1)) : null;
      if (gates != null)
      {
        await gates.WaitAsync(ct);
      }

      UserSession? session;
      try
      {
        if (!sessionId.HasValue)
        {
          session = await CreateSessionAsync();
        }
        else
        {
          session = await _userSessionRepository.GetByIdAsync(sessionId.Value, ct);
        }

        if (session == null || session.Refresh(userAgent, ipAddress).IsFailure)
        {
          session = await CreateSessionAsync();
        }
        else
        {
          _userSessionRepository.Update(session);
        }
      }
      finally
      {
        gates?.Release();
        if (gatesKey != null)
        {
          Gates.TryRemove(gatesKey, out _);
        }
      }


      return session.Id;

      async ValueTask<UserSession> CreateSessionAsync() =>
        await _userSessionRepository.CreateAsync(new UserSession(storeId, userId, userAgent, ipAddress), ct);
    }
  }
}