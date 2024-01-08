using ProjectIndustries.Sellify.Core.FileStorage.Image;
using ProjectIndustries.Sellify.Core.FileStorage.Image.ResizeStrategies;
using SkiaSharp;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem.Image.ResizeStrategies
{
  public class ImageResizeStrategyNoneExecutor : IImageResizeStrategyExecutor
  {
    public ImageResizeStrategy Strategy => ImageResizeStrategy.None;

    public SKBitmap Execute(OptimizationImageContext context, SKBitmap convertedBitmap)
    {
      return convertedBitmap;
    }
  }
}