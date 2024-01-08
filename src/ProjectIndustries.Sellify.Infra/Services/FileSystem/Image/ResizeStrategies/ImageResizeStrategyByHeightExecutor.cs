using ProjectIndustries.Sellify.Core.FileStorage.Image;
using ProjectIndustries.Sellify.Core.FileStorage.Image.ResizeStrategies;
using SkiaSharp;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem.Image.ResizeStrategies
{
  public class ImageResizeStrategyByHeightExecutor
    : ImageResizeStrategyByWidthHeightExecutor
  {
    public override ImageResizeStrategy Strategy => ImageResizeStrategy.ByHeight;

    protected override ImageResizeBy ResizeBy => ImageResizeBy.Height;

    protected override int GetDestinationValue(ImageSize size)
    {
      return size.Height;
    }

    protected override bool ResizeRequired(SKBitmap convertedBitmap, ImageSize size, OptimizationImageContext context)
    {
      if (context.ResizeToFitExactSize)
      {
        return convertedBitmap.Height != size.Height;
      }

      return convertedBitmap.Height > size.Height;
    }
  }
}