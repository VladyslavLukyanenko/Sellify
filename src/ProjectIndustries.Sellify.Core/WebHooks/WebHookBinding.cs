using System;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core.WebHooks
{
  public class WebHookBinding : TimestampAuditableEntity, IStoreBoundEntity
  {
    private WebHookBinding()
    {
    }

    public WebHookBinding(string eventType, Uri listenerEndpoint, Guid storeId, WebhookReceiverType receiverType)
    {
      EventType = eventType;
      ListenerEndpoint = listenerEndpoint;
      StoreId = storeId;
      ReceiverType = receiverType;
    }

    public string EventType { get; set; } = null!;
    public Uri ListenerEndpoint { get; set; } = null!;
    public Guid StoreId { get; set; }
    public WebhookReceiverType ReceiverType { get; set; } = WebhookReceiverType.CustomApi;
  }
}