using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.FileStorage.Config;
using ProjectIndustries.Sellify.Core.FileStorage.FileSystem;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem
{
  public interface IBinaryDataProcessingPipeline
  {
    bool CanProcess(FileUploadsConfig fileCfg, IBinaryData data);

    Task<IBinaryData> ProcessAsync(FileUploadsConfig fileCfg, IBinaryData data, CancellationToken token = default);
  }
}