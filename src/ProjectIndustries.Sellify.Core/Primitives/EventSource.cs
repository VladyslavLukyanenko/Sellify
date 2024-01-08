using System.Collections.Generic;
using ProjectIndustries.Sellify.Core.Events;

namespace ProjectIndustries.Sellify.Core.Primitives
{
  public abstract class EventSource : IEventSource
  {
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent eventItem)
    {
      _domainEvents.Add(eventItem);
    }

    public void RemoveDomainEvent(IDomainEvent eventItem)
    {
      _domainEvents.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
      _domainEvents.Clear();
    }
  }
}