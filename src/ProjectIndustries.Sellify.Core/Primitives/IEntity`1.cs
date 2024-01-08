namespace ProjectIndustries.Sellify.Core.Primitives
{
  public interface IEntity<out TKey>
  {
    TKey Id { get; }
  }
}