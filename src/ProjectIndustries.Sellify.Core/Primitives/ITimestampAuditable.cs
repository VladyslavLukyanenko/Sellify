using NodaTime;

namespace ProjectIndustries.Sellify.Core.Primitives
{
  public interface ITimestampAuditable
  {
    Instant CreatedAt { get; }
    Instant UpdatedAt { get; }
  }
}