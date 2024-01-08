using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using ProjectIndustries.Sellify.Core.Events;

namespace ProjectIndustries.Sellify.Infra
{
  public class MassTransitBasedMessageDispatcher : IMessageDispatcher
  {
    private readonly IBus _bus;

    public MassTransitBasedMessageDispatcher(IBus bus)
    {
      _bus = bus;
    }

    public async ValueTask PublishEventAsync<T>(T @event, CancellationToken ct = default)
    {
      await _bus.Publish(@event, ct);
    }

    public async ValueTask SendCommandAsync<T>(T @event, CancellationToken ct = default)
    {
      await _bus.Send(@event, ct);
    }
  }
}