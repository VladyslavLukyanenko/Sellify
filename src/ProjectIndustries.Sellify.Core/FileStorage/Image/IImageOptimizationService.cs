using System.Threading;
using System.Threading.Tasks;
using ProjectIndustries.Sellify.Core.FileStorage.FileSystem;

namespace ProjectIndustries.Sellify.Core.FileStorage.Image
{
  public interface IImageOptimizationService
  {
    Task<IBinaryData> OptimizeAsync(OptimizationImageContext context, CancellationToken token = default);
  }
}