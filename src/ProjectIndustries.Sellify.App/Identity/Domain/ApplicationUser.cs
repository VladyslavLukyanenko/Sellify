using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using ProjectIndustries.Sellify.Core.Events;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.App.Identity.Domain
{
  public class ApplicationUser : IdentityUser, IEventSource
  {
    private readonly List<IDomainEvent> _domainEvents = new();

    private ApplicationUser()
    {
    }

    public ApplicationUser(string email, string firstName, string lastName)
    {
      UserName = email;
      Email = email;
      FirstName = firstName;
      LastName = lastName;

      AddDomainEvent(new ApplicationUserCreated(Id, Email));
    }

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

    public string? Picture { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
  }

  public class ApplicationUserCreated : DomainEvent
  {
    public ApplicationUserCreated(string id, string email)
    {
      Id = id;
      Email = email;
    }

    public string Id { get; private set; }
    public string Email { get; private set; }
  }
}