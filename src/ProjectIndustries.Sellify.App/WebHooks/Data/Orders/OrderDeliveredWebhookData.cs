using ProjectIndustries.Sellify.Core.WebHooks;

namespace ProjectIndustries.Sellify.App.WebHooks.Orders
{
  [WebHookType("orders.delivered")]
  public class OrderDeliveredWebhookData : WebHookDataBase
  {
  }
}