// using System.Text;
// using System.Threading;
// using System.Threading.Tasks;
//
// namespace ProjectIndustries.Sellify.Core.WebHooks.Services
// {
//   public class TelegramWebHookPayloadFactory : IWebHookPayloadFactory
//   {
//     private readonly IWebhookDataSerializer _webhookDataSerializer;
//
//     public TelegramWebHookPayloadFactory(IWebhookDataSerializer webhookDataSerializer)
//     {
//       _webhookDataSerializer = webhookDataSerializer;
//     }
//
//     public bool CanCreate(WebhookReceiverType receiverType) => receiverType == WebhookReceiverType.Telegram;
//
//     public ValueTask<WebHookPayloadEnvelop> CreateAsync<T>(WebHookBinding binding, WebHooksConfig cfg, T data,
//       CancellationToken ct = default)
//       where T : WebHookDataBase
//     {
//       var dataEntries = _webhookDataSerializer.Serialize(data);
//
//       var message = new StringBuilder();
//       message.AppendFormat("<b>{0}</b>\n\n", data.Type);
//       foreach (var e in dataEntries)
//       {
//         if (string.IsNullOrEmpty(e.Key))
//         {
//           message.AppendLine();
//         }
//         else
//         {
//           message.AppendFormat("<b>{0}</b>: {1}\n", e.Key, e.Value);
//         }
//       }
//
//       var envelop = new WebHookPayloadEnvelop(message.ToString(), cfg, binding.ListenerEndpoint);
//       return ValueTask.FromResult(envelop);
//     }
//   }
// }