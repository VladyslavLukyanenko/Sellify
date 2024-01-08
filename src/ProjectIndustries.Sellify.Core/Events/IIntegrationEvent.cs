using NodaTime;

namespace ProjectIndustries.Sellify.Core.Events
{
  public interface IIntegrationEvent
  {
    Instant Timestamp { get; }
  }
}