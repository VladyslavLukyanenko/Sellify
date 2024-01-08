using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Payments.Data;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services
{
  public class MD5HashBasedSkrillWebhookValidator : ISkrillWebhookValidator
  {
    public Result<bool> IsValid(SkrillIntegrationConfig config, SkrillWebhookData data)
    {
      if (config.IsEmpty())
      {
        return Result.Failure<bool>("Skrill is not configured");
      }

      var secretHash = GetMD5String(config.Secret!);

      var payloadBuilder = new StringBuilder();
      var payload = payloadBuilder.Append(data.MerchantID)
        .Append(data.TransactionId)
        .Append(secretHash.ToUpper())
        .Append(data.Amount.ToString(CultureInfo.InvariantCulture))
        .Append(data.Currency)
        .Append((int) data.Status)
        .ToString();

      return GetMD5String(payload) == data.Md5Signature;
    }

    private string GetMD5String(string input) =>
      string.Join("", MD5.HashData(Encoding.ASCII.GetBytes(input)).Select(b => b.ToString("X2")));
  }
}