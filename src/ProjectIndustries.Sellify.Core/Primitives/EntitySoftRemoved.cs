using ProjectIndustries.Sellify.Core.Events;

namespace ProjectIndustries.Sellify.Core.Primitives
{
  public class EntitySoftRemoved : DomainEvent
  {
    public EntitySoftRemoved(ISoftRemovable source)
    {
      Source = source;
    }
    
    public ISoftRemovable Source { get; }
  }
}