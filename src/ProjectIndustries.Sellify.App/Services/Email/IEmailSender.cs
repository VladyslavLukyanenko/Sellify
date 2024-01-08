using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.App.Services.Email
{
  public interface IEmailSender
  {
    Task SendAsync(EmailMessage message, CancellationToken ct = default);
  }
}