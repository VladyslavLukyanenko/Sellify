using System;

namespace ProjectIndustries.Sellify.App.WebHooks.Data
{
  public class SaveBindingCommand
  {
    public string ReceiverType { get; set; } = null!;
    public string EventType { get; set; } = null!;
    public Uri ListenerEndpoint { get; set; } = null!;
  }
}