using NodaTime;

namespace ProjectIndustries.Sellify.Core.Events
{
  public abstract class DomainEvent : IDomainEvent
  {
    public Instant Timestamp { get; } = SystemClock.Instance.GetCurrentInstant();
  }
}