using CSharpFunctionalExtensions;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.WebApi.Payments.Data;
using ProjectIndustries.Sellify.WebApi.Payments.Domain;

namespace ProjectIndustries.Sellify.WebApi.Payments.Services
{
  public interface ISkrillWebhookValidator
  {
    Result<bool> IsValid(SkrillIntegrationConfig config, SkrillWebhookData data);
  }
}
