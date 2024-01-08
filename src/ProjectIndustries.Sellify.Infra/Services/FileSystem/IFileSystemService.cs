using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem
{
  public interface IFileSystemService
  {
    Task<string> SaveBinaryAsync(string fullPath, string fileName, Stream fileInputStream,
      CancellationToken cancellationToken = default);

    Task<Base64FileData> ReadAsBase64Async(string fullPath, CancellationToken token = default);
    Task CopyFileAsync(string source, string destination, CancellationToken token = default);
  }
}