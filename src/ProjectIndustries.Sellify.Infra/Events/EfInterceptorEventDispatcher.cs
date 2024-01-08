using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ProjectIndustries.Sellify.Core.Events;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Infra.Events
{
  public class EfInterceptorEventDispatcher : SaveChangesInterceptor
  {
    private readonly IMessageDispatcher _messageDispatcher;

    public EfInterceptorEventDispatcher(IMessageDispatcher messageDispatcher)
    {
      _messageDispatcher = messageDispatcher;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
      InterceptionResult<int> result, CancellationToken ct = default)
    {
      var context = eventData.Context;
      if (!context.ChangeTracker.HasChanges())
      {
        return result;
      }

      var domainEntries = context.ChangeTracker
        .Entries<IEventSource>()
        .Where(_ => _.Entity.DomainEvents.Any());

      foreach (var entry in domainEntries)
      {
        foreach (IDomainEvent domainEvent in entry.Entity.DomainEvents)
        {
          await _messageDispatcher.PublishEventAsync(domainEvent, ct);
        }

        entry.Entity.ClearDomainEvents();
      }

      return result;
    }
  }
}