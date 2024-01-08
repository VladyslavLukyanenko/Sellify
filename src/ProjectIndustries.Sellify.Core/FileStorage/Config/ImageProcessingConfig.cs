using ProjectIndustries.Sellify.Core.FileStorage.Image.ResizeStrategies;

namespace ProjectIndustries.Sellify.Core.FileStorage.Config
{
  public class ImageProcessingConfig
  {
    public string TargetImageFormat { get; set; } = null!;
    public ImageSize Size { get; set; } = null!;
    public bool ResizeToFitExactSize { get; set; }
    public ImageResizeStrategy ResizeStrategy { get; set; }
  }
}