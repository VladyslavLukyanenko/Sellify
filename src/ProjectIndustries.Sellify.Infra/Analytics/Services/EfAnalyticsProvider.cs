using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using ProjectIndustries.Sellify.App.Analytics.Model;
using ProjectIndustries.Sellify.App.Analytics.Services;
using ProjectIndustries.Sellify.App.Model;
using ProjectIndustries.Sellify.Core.Analytics;
using ProjectIndustries.Sellify.Core.Customers;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.Infra.Analytics.Services
{
  public class EfAnalyticsProvider : DataProvider, IAnalyticsProvider
  {
    private readonly IQueryable<Customer> _customers;
    private readonly IQueryable<UserSession> _userSessions;
    private readonly IQueryable<Order> _orders;

    public EfAnalyticsProvider(DbContext context)
      : base(context)
    {
      _customers = GetDataSource<Customer>();
      _userSessions = GetDataSource<UserSession>();
      _orders = GetDataSource<Order>();
    }

    public async ValueTask<Result<GeneralAnalytics>> GetGeneralAnalyticsAsync(Guid storeId,
      GeneralAnalyticsRequest request, CancellationToken ct = default)
    {
      var period = Enumeration.FromDisplayName<DataQueryPeriod>(request.Period);
      var offset = request.Offset;
      var totalResult = period.GetTotalRange(request.Start, offset);
      if (totalResult.IsFailure)
      {
        return totalResult.ConvertFailure<GeneralAnalytics>();
      }

      var (startOfPrev, startTime, endTime) = totalResult.Value;

      var incomes = await AggregateIncomesAsync(storeId, startTime, endTime, offset, ct);
      var revenue = await CalculateRevenueAsync(storeId, startOfPrev, startTime, endTime, ct);
      var visitorsCount = await CalculateVisitorsCountAsync(storeId, startOfPrev, endTime, startTime, ct);
      var totalCustomers = await CalculateCountsAsync(_customers.Where(_ => _.StoreId == storeId), startOfPrev,
        startTime, endTime, ct);
      var totalOrders =
        await CalculateCountsAsync(_orders.Where(_ => _.StoreId == storeId), startOfPrev, startTime, endTime, ct);
      var analytics = new GeneralAnalytics
      {
        TotalCustomers = totalCustomers,
        TotalRevenue = revenue,
        TotalOrders = totalOrders,
        Income = incomes,
        VisitorsCount = visitorsCount
      };

      return analytics;
    }

    private async ValueTask<ValueDiff<int>> CalculateVisitorsCountAsync(Guid storeId, Instant? startOfPrev,
      Instant? endTime, Instant? startTime, CancellationToken ct)
    {
      var userSessionsQuery = _userSessions.Where(_ => _.StoreId == storeId);
      if (startTime.HasValue)
      {
        userSessionsQuery = userSessionsQuery.Where(_ => _.StartedAt >= startOfPrev && _.StartedAt <= endTime);
      }

      var counts = await userSessionsQuery.GroupBy(_ => _.UserId == null ? (string) (object) _.Id : _.UserId)
        .Select(_ => new
        {
          Id = _.Key,
          StartedAt = _.Min(r => r.StartedAt),
          LastActivityAt = _.Max(r => r.LastActivityAt)
        })
        .Select(_ => new
        {
          _.Id,
          IsCurr = !startTime.HasValue || _.StartedAt >= startTime || _.LastActivityAt >= startTime
        })
        .GroupBy(_ => _.IsCurr)
        .Select(_ => new {IsCurr = _.Key, Count = _.Count()})
        .ToDictionaryAsync(_ => _.IsCurr, _ => _.Count, ct);

      counts.TryGetValue(true, out var curr);
      counts.TryGetValue(false, out var prev);
      return ValueDiff<int>.CreateInt32(curr, endTime.HasValue ? prev : null);
    }

    private async ValueTask<ValueDiff<int>> CalculateCountsAsync<T>(IQueryable<T> source, Instant? startOfPrev,
      Instant? startTime, Instant? endTime, CancellationToken ct) where T : ITimestampAuditable
    {
      if (endTime.HasValue && startOfPrev.HasValue)
      {
        source = source.Where(_ => _.CreatedAt >= startOfPrev && _.CreatedAt <= endTime);
      }

      var counts = await source.Select(_ => new {IsCurr = !startTime.HasValue || _.CreatedAt >= startTime})
        .GroupBy(_ => _.IsCurr)
        .Select(_ => new {IsCurr = _.Key, Count = _.Count()})
        .ToDictionaryAsync(_ => _.IsCurr, _ => _.Count, ct);

      counts.TryGetValue(true, out var curr);
      counts.TryGetValue(false, out var prev);
      return ValueDiff<int>.CreateInt32(curr, endTime.HasValue ? prev : null);
    }

    private async ValueTask<ValueDiff<decimal>> CalculateRevenueAsync(Guid storeId, Instant? startOfPrev,
      Instant? startTime, Instant? endTime, CancellationToken ct)
    {
      var source = _orders.Where(_ => _.StoreId == storeId && _.Status == OrderStatus.Fulfilled);
      if (endTime.HasValue && startOfPrev.HasValue)
      {
        source = source.Where(_ => _.CreatedAt >= startOfPrev && _.CreatedAt <= endTime);
      }

      var counts = await source
        .Select(_ => new
        {
          IsCurr = !startTime.HasValue || _.CreatedAt >= startTime,
          Price = _.Product.Price * _.Product.Quantity
        })
        .GroupBy(_ => _.IsCurr)
        .Select(_ => new {IsCurr = _.Key, Sum = _.Sum(r => r.Price)})
        .ToDictionaryAsync(_ => _.IsCurr, _ => _.Sum, ct);

      counts.TryGetValue(true, out var curr);
      counts.TryGetValue(false, out var prev);
      return ValueDiff<decimal>.CreateDecimal(curr, endTime.HasValue ? prev : null);
    }

    private async ValueTask<List<IncomeStatsItem>> AggregateIncomesAsync(Guid storeId, Instant? startTime,
      Instant? endTime, Offset offset, CancellationToken ct)
    {
      var ordersQuery = _orders.Where(_ => _.StoreId == storeId && _.Status == OrderStatus.Fulfilled);
      if (endTime.HasValue && startTime.HasValue)
      {
        ordersQuery = ordersQuery.Where(_ => _.CreatedAt >= startTime && _.CreatedAt <= endTime);
      }

      var sells = await ordersQuery
          .Select(_ => new
          {
            Price = _.Product.Price * _.Product.Quantity,
            _.CreatedAt
          })
          .ToArrayAsync(ct);

      var incomes = sells.GroupBy(_ => _.CreatedAt.WithOffset(offset).Date)
        .Select(i => new IncomeStatsItem
        {
          Date = i.Key,
          Amount = i.Sum(_ => _.Price)
        })
        .OrderBy(_ => _.Date)
        .ToList();

      return incomes;
    }
  }
}