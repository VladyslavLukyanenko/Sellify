namespace ProjectIndustries.Sellify.App.Services.Email
{
  public interface IEmailMessageFactory
  {
    EmailMessage Create(string subject, string content, params string[] receiverEmails);
  }
}