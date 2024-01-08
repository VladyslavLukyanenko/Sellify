using System;

namespace ProjectIndustries.Sellify.Core
{
  public interface IStoreBoundEntity
  {
    Guid StoreId { get; }
  }
}