using System.Collections.Generic;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public interface IWebhookDataSerializer
  {
    IEnumerable<KeyValuePair<string?, string?>> Serialize(WebHookDataBase data);
  }
}