namespace ProjectIndustries.Sellify.Core.Audit.Services
{
  public interface IMutableChangeSetProvider
    : ICurrentChangeSetProvider
  {
    void SetChangeSet(ChangeSet changeSet);
  }
}