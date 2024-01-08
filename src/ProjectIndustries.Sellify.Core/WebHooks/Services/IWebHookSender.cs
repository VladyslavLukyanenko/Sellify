using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public interface IWebHookSender
  {
    ValueTask<Result> SendAsync(WebHookPayloadEnvelop envelop, CancellationToken ct = default);
  }
}