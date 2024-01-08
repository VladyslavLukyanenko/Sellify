using System.Collections.Generic;
using ProjectIndustries.Sellify.Core.Events;

namespace ProjectIndustries.Sellify.Core.Primitives
{
  public interface IEventSource
  {
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void AddDomainEvent(IDomainEvent eventItem);
    void RemoveDomainEvent(IDomainEvent eventItem);
    void ClearDomainEvents();
  }
}