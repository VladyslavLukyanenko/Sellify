namespace ProjectIndustries.Sellify.Core.Services
{
  public interface IUserAgentService
  {
    UserAgentDeviceType ResolveDeviceType(string userAgent);
  }
}