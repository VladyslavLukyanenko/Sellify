using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.App.Model;
using ProjectIndustries.Sellify.App.WebHooks.Data;
using ProjectIndustries.Sellify.App.WebHooks.Orders;
using ProjectIndustries.Sellify.App.WebHooks.Services;
using ProjectIndustries.Sellify.Core.Collections;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Infra.Services;

namespace ProjectIndustries.Sellify.Infra.WebHooks.Services
{
  public class WebhooksProvider : DataProvider, IWebhooksProvider
  {
    private static readonly string[] SupportedWebhooks;
    private readonly IQueryable<WebHookBinding> _bindings;

    static WebhooksProvider()
    {
      SupportedWebhooks = typeof(OrderCreatedWebhookData).Assembly.ExportedTypes
        .Where(type => typeof(WebHookDataBase).IsAssignableFrom(type))
        .Select(_ => _.GetCustomAttribute<WebHookTypeAttribute>())
        .Where(a => a != null)
        .Select(_ => _!.Name)
        .ToArray();
    }

    public WebhooksProvider(DbContext context) : base(context)
    {
      _bindings = GetDataSource<WebHookBinding>();
    }

    public IReadOnlyCollection<string> GetSupportedWebhooks() => SupportedWebhooks;

    public async ValueTask<IPagedList<WebhookRowData>> GetBindingRowsPageAsync(Guid storeId,
      FilteredPageRequest pageRequest, CancellationToken ct = default)
    {
      var query = _bindings.Where(_ => _.StoreId == storeId);
      if (!pageRequest.IsSearchTermEmpty())
      {
        var term = pageRequest.SearchTerm!.ToUpper();
        query = query.Where(_ => _.EventType.ToUpper().Contains(term));
      }

      var page = await query
        .OrderByDescending(_ => _.CreatedAt)
        .PaginateAsync(pageRequest, ct);

      return page.ProjectTo(r => new WebhookRowData
      {
        Id = r.Id,
        CreatedAt = r.CreatedAt,
        EventType = r.EventType,
        ListenerEndpoint = r.ListenerEndpoint.ToString(),
        ReceiverType = r.ReceiverType.Name
      });
    }
  }
}