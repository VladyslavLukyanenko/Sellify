using ProjectIndustries.Sellify.Core.WebHooks;

namespace ProjectIndustries.Sellify.App.WebHooks.Orders
{
  [WebHookType("orders.fulfilled")]
  public class OrderFulfilledWebhookData : WebHookDataBase
  {
  }
}