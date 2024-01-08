using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.FileStorage.Config;

namespace ProjectIndustries.Sellify.Core.FileStorage.FileSystem
{
  public interface IFileUploadService
  {
    Task<StoredBinaryData> StoreAsync(IBinaryData data, IEnumerable<FileUploadsConfig> configs,
      string? oldFileName = null, CancellationToken ct = default);

    Task<StoredBinaryData> StoreAsync(IBinaryData data, IEnumerable<FileUploadsConfig> configs,
      IFileNameProvider fileNameProvider, CancellationToken ct = default);
  }
}