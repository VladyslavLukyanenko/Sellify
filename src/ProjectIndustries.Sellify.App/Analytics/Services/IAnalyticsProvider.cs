using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.App.Analytics.Model;

namespace ProjectIndustries.Sellify.App.Analytics.Services
{
  public interface IAnalyticsProvider
  {
    ValueTask<Result<GeneralAnalytics>> GetGeneralAnalyticsAsync(Guid storeId, GeneralAnalyticsRequest request,
      CancellationToken ct = default);
  }
}