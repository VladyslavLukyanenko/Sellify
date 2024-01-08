using System;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core
{
  public abstract class StoreBoundEntity<T> : SoftRemovableEntity<T>, IStoreBoundEntity
    where T : IComparable<T>, IEquatable<T>
  {
    public Guid StoreId { get; protected set; }
  }

  public abstract class StoreBoundEntity : StoreBoundEntity<long>
  {
  }
}