using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;

namespace ProjectIndustries.Sellify.Infra.WebHooks.Services
{
  public class HmacWebHookPayloadSignatureCalculator : IWebHookPayloadSignatureCalculator
  {
    public ValueTask<string> CalculateSignature(string serializedPayload, string eventType, WebHooksConfig config,
      CancellationToken ct = default)
    {
      var secret = Encoding.UTF8.GetBytes(config.ClientSecret);
      var payload = Encoding.UTF8.GetBytes(serializedPayload);

      var algorithm = new HMACSHA256(secret);
      var hash = algorithm.ComputeHash(payload);
      var signature = Convert.ToBase64String(hash);

      return ValueTask.FromResult(signature);
    }
  }
}