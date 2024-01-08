using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.FileStorage.Config;
using ProjectIndustries.Sellify.Core.FileStorage.FileSystem;
using ProjectIndustries.Sellify.Core.FileStorage.Image;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem.Image
{
  public class ImageProcessingPipeline : IBinaryDataProcessingPipeline
  {
    private readonly IImageOptimizationService _imageOptimizationService;
    private readonly IMimeTypeResolver _mimeTypeResolver;

    public ImageProcessingPipeline(IImageOptimizationService imageOptimizationService,
      IMimeTypeResolver mimeTypeResolver)
    {
      _imageOptimizationService = imageOptimizationService;
      _mimeTypeResolver = mimeTypeResolver;
    }

    public bool CanProcess(FileUploadsConfig cfg, IBinaryData data)
    {
      return cfg.ImageProcessing != null && _mimeTypeResolver.IsImage(data.ContentType);
    }

    public Task<IBinaryData> ProcessAsync(FileUploadsConfig fileCfg, IBinaryData data,
      CancellationToken token = default)
    {
      var processingCfg = fileCfg.ImageProcessing;
      if (processingCfg == null)
      {
        return Task.FromResult(data);
      }

      var imageContext = new OptimizationImageContext(data, processingCfg.Size, processingCfg.ResizeStrategy,
        processingCfg.ResizeToFitExactSize, processingCfg.TargetImageFormat);

      return _imageOptimizationService.OptimizeAsync(imageContext, token);
    }
  }
}