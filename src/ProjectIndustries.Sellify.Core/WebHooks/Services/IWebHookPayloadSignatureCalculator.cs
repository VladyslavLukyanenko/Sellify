using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public interface IWebHookPayloadSignatureCalculator
  {
    ValueTask<string> CalculateSignature(string serializedPayload, string eventType, WebHooksConfig config,
      CancellationToken ct = default);
  }
}