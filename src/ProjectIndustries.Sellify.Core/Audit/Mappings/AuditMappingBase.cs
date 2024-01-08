using System;
using System.Collections.Generic;
using System.Linq;
using ProjectIndustries.Sellify.Core.Audit.Processors;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Core.Audit.Mappings
{
  public abstract class AuditMappingBase
    : IAuditMappingProvider
  {
    private readonly IDictionary<Type, IEntityMappingBuilder> _builders =
      new Dictionary<Type, IEntityMappingBuilder>();

    IDictionary<Type, IEntityMappingBuilder> IAuditMappingProvider.Builders =>
      _builders.Keys.ToDictionary(type => type, type => _builders[type]);

    protected AuditEntityMappingBuilder<TEntity, TId> Map<TEntity, TId>()
      where TEntity : IEntity<TId>
    {
      var builder = new AuditEntityMappingBuilder<TEntity, TId>(typeof(NullAuditEntityPreProcessor));

      _builders[typeof(TEntity)] = builder;
      return builder;
    }

    protected AuditEntityMappingBuilder<TEntity, long> Map<TEntity>()
      where TEntity : Entity
    {
      return Map<TEntity, long>();
    }
  }
}