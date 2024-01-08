using System.Collections.Generic;
using System.Linq;
using ProjectIndustries.Sellify.Core.FileStorage.Image.ResizeStrategies;

namespace ProjectIndustries.Sellify.Infra.Services.FileSystem.Image.ResizeStrategies
{
  public class DefaultImageResizeStrategyExecutorProvider : IImageResizeStrategyExecutorProvider
  {
    private readonly IEnumerable<IImageResizeStrategyExecutor> _executors;

    public DefaultImageResizeStrategyExecutorProvider(IEnumerable<IImageResizeStrategyExecutor> executors)
    {
      _executors = executors;
    }

    public IImageResizeStrategyExecutor? Get(ImageResizeStrategy strategy)
    {
      return _executors.FirstOrDefault(_ => _.Strategy == strategy);
    }
  }
}