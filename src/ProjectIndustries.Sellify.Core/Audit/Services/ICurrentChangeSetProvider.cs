namespace ProjectIndustries.Sellify.Core.Audit.Services
{
  public interface ICurrentChangeSetProvider
  {
    ChangeSet? CurrentChangSet { get; }
  }
}