using System;

// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace ProjectIndustries.Sellify.Core.Primitives
{
  public abstract class AuditableEntity<TKey>
    : TimestampAuditableEntity<TKey>, IAuthorAuditable
    where TKey : IComparable<TKey>, IEquatable<TKey>
  {
    protected AuditableEntity()
    {
    }

    protected AuditableEntity(TKey id)
      : base(id)
    {
    }

    public string? UpdatedBy { get; private set; }
    public string? CreatedBy { get; private set; }
  }
}