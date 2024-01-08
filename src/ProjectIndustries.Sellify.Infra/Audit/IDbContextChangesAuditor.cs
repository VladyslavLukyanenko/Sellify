using Microsoft.EntityFrameworkCore;

namespace ProjectIndustries.Sellify.Infra.Audit
{
  public interface IDbContextChangesAuditor
  {
    void AuditChanges(DbContext ctx);
  }
}