using System;
using ProjectIndustries.Sellify.Core.Audit.Mappings;

namespace ProjectIndustries.Sellify.Core.Audit.Services
{
  public interface IChangeSetMapperProvider
  {
    IEntityToChangeSetEntryMapper GetMapper(Type entityType);
    IEntityToChangeSetEntryMapper GetMapper(string entityType);
    bool HasMapperForType(Type type);
    bool HasMapperForType(string entityType);
  }
}