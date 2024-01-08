using ProjectIndustries.Sellify.Core.Audit.Mappings;

namespace ProjectIndustries.Sellify.Core.Audit.Services
{
  public interface IEntityToChangeSetEntryMapperFactory
  {
    IEntityToChangeSetEntryMapper Create(IEntityMappingBuilder builder);
  }
}