using CSharpFunctionalExtensions;
using NodaTime;

namespace ProjectIndustries.Sellify.Core.Primitives
{
  public interface ISoftRemovable
  {
    Instant RemovedAt { get; }
    bool IsRemoved();
    Result Remove();
    // Result RemoveAt(Instant removedAt);
  }
}