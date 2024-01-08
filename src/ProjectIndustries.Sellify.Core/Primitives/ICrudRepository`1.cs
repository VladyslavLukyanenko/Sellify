namespace ProjectIndustries.Sellify.Core.Primitives
{
  public interface ICrudRepository<T>
    : ICrudRepository<T, long>
    where T : class, IEntity<long>
  {
  }
}