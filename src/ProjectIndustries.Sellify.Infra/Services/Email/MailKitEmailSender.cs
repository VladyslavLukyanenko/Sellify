﻿using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using ProjectIndustries.Sellify.App.Config;
using ProjectIndustries.Sellify.App.Services;
using ProjectIndustries.Sellify.App.Services.Email;

namespace ProjectIndustries.Sellify.Infra.Services.Email
{
  public class MailKitEmailSender
    : IEmailSender
  {
    private readonly CommonConfig _config;
    private readonly IViewRenderService _viewRenderService;

    public MailKitEmailSender(IOptions<CommonConfig> config,
      IViewRenderService viewRenderService)
    {
      _viewRenderService = viewRenderService;
      _config = config.Value;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken ct = default)
    {
      using (var smtp = new SmtpClient())
      {
        var emailConfig = _config.EmailNotifications;
        var options = (SecureSocketOptions) emailConfig.SmtpSecurity;
        await smtp.ConnectAsync(emailConfig.SmtpHost, emailConfig.SmtpPort, options, ct);

        var credentials = new SaslMechanismLogin(emailConfig.SenderEmail, emailConfig.SenderPassword);
        await smtp.AuthenticateAsync(credentials, ct);

        var mimeMessage = message.ToMimeMessage();
        mimeMessage.Body = new TextPart(TextFormat.Html)
        {
          Text = await _viewRenderService.RenderAsync(emailConfig.EmailTemplate, message)
        };

        await smtp.SendAsync(mimeMessage, ct);
        await smtp.DisconnectAsync(true, ct);
      }
    }
  }
}