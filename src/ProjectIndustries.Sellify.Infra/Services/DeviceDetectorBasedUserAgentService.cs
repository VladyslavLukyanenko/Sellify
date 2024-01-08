using DeviceDetectorNET;
using ProjectIndustries.Sellify.Core.Services;

namespace ProjectIndustries.Sellify.Infra.Services
{
  public class DeviceDetectorBasedUserAgentService : IUserAgentService
  {
    // todo: consider to use cache here. but better to use with expiration
    // private static readonly ICache Cache = new DictionaryCache();

    public UserAgentDeviceType ResolveDeviceType(string userAgent)
    {
      var detector = new DeviceDetector(userAgent);
      detector.Parse();

      if (detector.IsDesktop())
      {
        return UserAgentDeviceType.Desktop;
      }

      if (detector.IsMobile())
      {
        return UserAgentDeviceType.Mobile;
      }

      return UserAgentDeviceType.Unknown;
    }
  }
}