using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.Events
{
  public interface IMessageDispatcher
  {
    ValueTask PublishEventAsync<T>(T @event, CancellationToken ct = default);
    ValueTask SendCommandAsync<T>(T @event, CancellationToken ct = default);
  }
}