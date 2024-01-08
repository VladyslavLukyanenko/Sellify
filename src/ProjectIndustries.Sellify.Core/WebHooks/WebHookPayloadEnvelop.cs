using System;

namespace ProjectIndustries.Sellify.Core.WebHooks
{
  public class WebHookPayloadEnvelop
  {
    public WebHookPayloadEnvelop()
    {
    }

    public WebHookPayloadEnvelop(string payload, WebHooksConfig cfg, Uri? listenerEndpoint)
    {
      Payload = payload;
      ListenerEndpoint = listenerEndpoint;
      StoreId = cfg.StoreId;
    }

    public string Payload { get; set; } = null!;
    public Guid StoreId { get; set; }
    public Uri? ListenerEndpoint { get; set; }
  }
}