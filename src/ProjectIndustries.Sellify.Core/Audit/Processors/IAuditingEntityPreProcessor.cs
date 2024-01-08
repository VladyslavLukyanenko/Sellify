namespace ProjectIndustries.Sellify.Core.Audit.Processors
{
  public interface IAuditingEntityPreProcessor
  {
    object PreProcess(object entity);
  }
}