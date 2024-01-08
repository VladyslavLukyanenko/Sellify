using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Infra.Repositories
{
  public abstract class EfCrudRepository<T> : EfCrudRepository<T, long>
    where T : class, IEntity<long>, IEventSource
  {
    protected EfCrudRepository(DbContext context, IUnitOfWork unitOfWork)
      : base(context, unitOfWork)
    {
    }
  }
}