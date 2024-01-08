using ProjectIndustries.Sellify.Core.FileStorage.Config;
using ProjectIndustries.Sellify.Core.FileStorage.FileSystem;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem
{
  public interface IBinaryDataProcessingPipelineProvider
  {
    IBinaryDataProcessingPipeline? GetPipeline(FileUploadsConfig cfg, IBinaryData data);
  }
}