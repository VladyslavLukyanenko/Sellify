using Microsoft.Extensions.Options;
using ProjectIndustries.Sellify.App.Config;
using ProjectIndustries.Sellify.App.Services.Email;

namespace ProjectIndustries.Sellify.Infra.Services.Email
{
  public class DefaultEmailMessageFactory : IEmailMessageFactory
  {
    private readonly CommonConfig _config;

    public DefaultEmailMessageFactory(IOptions<CommonConfig> config)
    {
      _config = config.Value;
    }

    public EmailMessage Create(string subject, string content, params string[] recipientEmails)
    {
      return new EmailMessage(_config.EmailNotifications.SenderEmail, subject, content, recipientEmails);
    }
  }
}