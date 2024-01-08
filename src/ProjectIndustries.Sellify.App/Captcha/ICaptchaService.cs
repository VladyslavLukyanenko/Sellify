using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace ProjectIndustries.Sellify.App.Captcha
{
  public interface ICaptchaService
  {
    ValueTask<Result> ValidateAsync(string captchaResponse);
  }
}