using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.App.Model;
using ProjectIndustries.Sellify.App.WebHooks.Data;
using ProjectIndustries.Sellify.Core.Collections;

namespace ProjectIndustries.Sellify.App.WebHooks.Services
{
  public interface IWebhooksProvider
  {
    IReadOnlyCollection<string> GetSupportedWebhooks();

    ValueTask<IPagedList<WebhookRowData>> GetBindingRowsPageAsync(Guid storeId, FilteredPageRequest pageRequest,
      CancellationToken ct = default);
  }
}