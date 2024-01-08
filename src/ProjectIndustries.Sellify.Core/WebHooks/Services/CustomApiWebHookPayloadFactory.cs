using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.Services;

namespace ProjectIndustries.Sellify.Core.WebHooks.Services
{
  public class CustomApiWebHookPayloadFactory : IWebHookPayloadFactory
  {
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IWebHookBindingRepository _webHookBindingRepository;
    private readonly IWebHookPayloadSignatureCalculator _signatureCalculator;

    public CustomApiWebHookPayloadFactory(IJsonSerializer jsonSerializer,
      IWebHookBindingRepository webHookBindingRepository,
      IWebHookPayloadSignatureCalculator signatureCalculator)
    {
      _jsonSerializer = jsonSerializer;
      _webHookBindingRepository = webHookBindingRepository;
      _signatureCalculator = signatureCalculator;
    }

    public bool CanCreate(WebhookReceiverType receiverType) => receiverType == WebhookReceiverType.CustomApi;

    public async ValueTask<WebHookPayloadEnvelop> CreateAsync<T>(WebHookBinding binding, WebHooksConfig cfg, T data,
      CancellationToken ct = default)
      where T : WebHookDataBase
    {
      var serializedData = await _jsonSerializer.SerializeAsync(data, ct);
      var signature = await _signatureCalculator.CalculateSignature(serializedData, binding.EventType, cfg, ct);

      var payload = new WebHookPayload(serializedData, signature, binding.EventType);

      var serializedPayload = await _jsonSerializer.SerializeAsync(payload, ct);
      return new WebHookPayloadEnvelop(serializedPayload, cfg, binding.ListenerEndpoint);
    }
  }
}