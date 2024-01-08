using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;

namespace ProjectIndustries.Sellify.Infra.WebHooks.Services
{
  public class HttpClientWebHookSender : IWebHookSender
  {
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IPublishedWebHookRepository _publishedWebHookRepository;
    private readonly IUnitOfWork _unitOfWork;

    public HttpClientWebHookSender(IHttpClientFactory httpClientFactory,
      IPublishedWebHookRepository publishedWebHookRepository, IUnitOfWork unitOfWork)
    {
      _httpClientFactory = httpClientFactory;
      _publishedWebHookRepository = publishedWebHookRepository;
      _unitOfWork = unitOfWork;
    }

    public async ValueTask<Result> SendAsync(WebHookPayloadEnvelop envelop, CancellationToken ct = default)
    {
      var client = _httpClientFactory.CreateClient(envelop.ListenerEndpoint!.Host);
      PublishedWebHook publishedWebHook;
      Result result;
      string? rawResponse = null;
      try
      {
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, envelop.ListenerEndpoint)
        {
          Content = new StringContent(envelop.Payload, Encoding.UTF8,
            "application/json")
        };

        var response = await client.SendAsync(requestMessage, ct);
        rawResponse = await response.Content.ReadAsStringAsync(ct);
        response.EnsureSuccessStatusCode();

        result = Result.Success();
        publishedWebHook = PublishedWebHook.Succeeded(envelop);
      }
      catch (Exception exc)
      {
        var errorMessage = !string.IsNullOrEmpty(rawResponse) ? rawResponse : exc.ToString();
        result = Result.Failure(errorMessage);
        publishedWebHook = PublishedWebHook.Failure(envelop, errorMessage);
      }

      await _publishedWebHookRepository.CreateAsync(publishedWebHook, ct);
      await _unitOfWork.SaveEntitiesAsync(ct);

      return result;
    }
  }
}