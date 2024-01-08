using System;
using System.Collections.Generic;

namespace ProjectIndustries.Sellify.Core.Audit.Mappings
{
  public interface IAuditMappingProvider
  {
    IDictionary<Type, IEntityMappingBuilder> Builders { get; }
  }
}