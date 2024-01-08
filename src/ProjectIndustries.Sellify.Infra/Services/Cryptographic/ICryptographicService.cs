using System.IO;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Infra.Services.Cryptographic
{
  public interface ICryptographicService
  {
    Task<string> ComputeHashAsync(Stream stream);
  }
}