using System;
using NodaTime;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core.WebHooks
{
  public class PublishedWebHook : Entity, IStoreBoundEntity
  {
    private PublishedWebHook()
    {
    }

    private PublishedWebHook(string payload, WebHookDeliveryStatus status, string? statusDescription,
      Guid storeId, Uri listenerEndpoint)
    {
      Payload = payload;
      Status = status;
      StatusDescription = statusDescription;
      StoreId = storeId;
      ListenerEndpoint = listenerEndpoint;
    }

    public static PublishedWebHook Succeeded(WebHookPayloadEnvelop envelop) =>
      new(envelop.Payload, WebHookDeliveryStatus.Delivered, null, envelop.StoreId, envelop.ListenerEndpoint!);

    public static PublishedWebHook Failure(WebHookPayloadEnvelop envelop, string reason) =>
      new(envelop.Payload, WebHookDeliveryStatus.Failed, reason, envelop.StoreId, envelop.ListenerEndpoint!);

    public string Payload { get; private set; } = null!;
    public WebHookDeliveryStatus Status { get; private set; }
    public string? StatusDescription { get; private set; }
    public Uri ListenerEndpoint { get; private set; } = null!;
    public Guid StoreId { get; private set; }


    public Instant Timestamp { get; private set; } = SystemClock.Instance.GetCurrentInstant();
  }
}