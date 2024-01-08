using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.Analytics.Services
{
  public interface IUserSessionService
  {
    ValueTask<Guid> RefreshOrCreateSessionAsync(Guid storeId, Guid? sessionId, string userAgent,
      IPAddress? ipAddress, string? userId, CancellationToken ct = default);
  }
}