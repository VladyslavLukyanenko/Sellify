using ProjectIndustries.Sellify.Core.FileStorage.Image;
using ProjectIndustries.Sellify.Core.FileStorage.Image.ResizeStrategies;
using SkiaSharp;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem.Image
{
  public interface IImageResizeStrategyExecutor
  {
    ImageResizeStrategy Strategy { get; }
    SKBitmap Execute(OptimizationImageContext context, SKBitmap convertedBitmap);
  }
}