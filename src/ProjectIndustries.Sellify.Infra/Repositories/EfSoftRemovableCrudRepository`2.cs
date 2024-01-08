using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Infra.Repositories
{
  public abstract class EfSoftRemovableCrudRepository<T, TKey> : EfCrudRepository<T, TKey>
    where T : class, ISoftRemovable, IEntity<TKey>, IEventSource
    where TKey : IComparable<TKey>, IEquatable<TKey>
  {
    protected EfSoftRemovableCrudRepository(DbContext context, IUnitOfWork unitOfWork)
      : base(context, unitOfWork)
    {
    }

    protected override IQueryable<T> DataSource => base.DataSource.WhereNotRemoved();
    protected IQueryable<T> UnfilteredDataSource => base.DataSource;

    public override void Remove(T aggregate)
    {
      aggregate.Remove();
      Update(aggregate);
    }

    public override void Remove(IEnumerable<T> aggregates)
    {
      foreach (var aggregate in aggregates)
      {
        Remove(aggregate);
      }
    }
  }
}