using System.Linq;
using NodaTime;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Infra
{
  // ReSharper disable once InconsistentNaming
  public static class IQueryableExtensions
  {
    public static IQueryable<T> WhereNotRemoved<T>(this IQueryable<T> src)
      where T: class, ISoftRemovable
    {
      return src.Where(_ => _.RemovedAt == Instant.MaxValue);
    }
    
  }
}