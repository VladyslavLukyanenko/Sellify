using ProjectIndustries.Sellify.Core.WebHooks;

namespace ProjectIndustries.Sellify.App.WebHooks.Orders
{
  [WebHookType("orders.created")]
  public class OrderCreatedWebhookData : WebHookDataBase
  {
  }
}