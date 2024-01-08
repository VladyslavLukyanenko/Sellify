using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using ProjectIndustries.Sellify.App;
using ProjectIndustries.Sellify.App.WebHooks;
using ProjectIndustries.Sellify.Core;
using ProjectIndustries.Sellify.Core.Events;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;

namespace ProjectIndustries.Sellify.Infra.WebHooks
{
  public class WebHookPayloadsPublisher : IPublishObserver, ISendObserver
  {
    private readonly IEnumerable<IWebHookPayloadMapper> _webHookPayloadMappers;
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly IWebHookBindingRepository _webHookBindingRepository;
    private readonly IEnumerable<IWebHookPayloadFactory> _webHookPayloadFactories;

    public WebHookPayloadsPublisher(IEnumerable<IWebHookPayloadMapper> webHookPayloadMappers,
      IMessageDispatcher messageDispatcher, IWebHookBindingRepository webHookBindingRepository,
      IEnumerable<IWebHookPayloadFactory> webHookPayloadFactories)
    {
      _webHookPayloadMappers = webHookPayloadMappers;
      _messageDispatcher = messageDispatcher;
      _webHookBindingRepository = webHookBindingRepository;
      _webHookPayloadFactories = webHookPayloadFactories;
    }

    public Task PrePublish<T>(PublishContext<T> context) where T : class => Task.CompletedTask;

    public async Task PostPublish<T>(PublishContext<T> context) where T : class
    {
      await PublishWebhookAsync(context.Message, context.CancellationToken);
    }

    public Task PublishFault<T>(PublishContext<T> context, Exception exception) where T : class => Task.CompletedTask;

    public Task PreSend<T>(SendContext<T> context) where T : class => Task.CompletedTask;

    public async Task PostSend<T>(SendContext<T> context) where T : class
    {
      await PublishWebhookAsync(context.Message, context.CancellationToken);
    }

    public Task SendFault<T>(SendContext<T> context, Exception exception) where T : class => Task.CompletedTask;

    private async ValueTask PublishWebhookAsync<T>(T message, CancellationToken ct) where T : class
    {
      if (message is not IStoreBoundEntity de)
      {
        return;
      }

      var mapper = _webHookPayloadMappers.FirstOrDefault(_ => _.CanMap(message));
      if (mapper is null)
      {
        return;
      }

      WebHookDataBase? data = await mapper.MapAsync(message, ct);
      if (data is null)
      {
        return;
      }

      // todo: do we need it?
      var cfg = await _webHookBindingRepository.GetConfigOfStoreAsync(de.StoreId, ct);
      if (cfg is null || !cfg.IsEnabled)
      {
        return;
      }

      var bindings = await _webHookBindingRepository.GetByTypeAsync(de.StoreId, data.Type, ct);
      foreach (var binding in bindings)
      {
        var factory = _webHookPayloadFactories.FirstOrDefault(_ => _.CanCreate(binding.ReceiverType));
        if (factory == null)
        {
          throw new AppException("Can't find webhook payload factory for receiver type " + binding.ReceiverType.Name);
        }

        var payload = await factory.CreateAsync(binding, cfg, data, ct);

        await _messageDispatcher.PublishEventAsync(payload, ct);
      }
    }
  }
}