using ProjectIndustries.Sellify.Core.FileStorage.Image.ResizeStrategies;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem.Image.ResizeStrategies
{
  public interface IImageResizeStrategyExecutorProvider
  {
    IImageResizeStrategyExecutor? Get(ImageResizeStrategy strategy);
  }
}