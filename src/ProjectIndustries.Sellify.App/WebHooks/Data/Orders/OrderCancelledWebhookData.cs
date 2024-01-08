using ProjectIndustries.Sellify.Core.WebHooks;

namespace ProjectIndustries.Sellify.App.WebHooks.Orders
{
  [WebHookType("orders.cancelled")]
  public class OrderCancelledWebhookData : WebHookDataBase
  {
  }
}