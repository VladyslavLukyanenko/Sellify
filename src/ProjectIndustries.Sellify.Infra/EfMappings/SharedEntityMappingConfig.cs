namespace ProjectIndustries.Sellify.Infra.EfMappings
{
  public abstract class SharedEntityMappingConfig<T> : EntityMappingConfig<T> where T : class
  {
    protected override string SchemaName => "public";
  }
}