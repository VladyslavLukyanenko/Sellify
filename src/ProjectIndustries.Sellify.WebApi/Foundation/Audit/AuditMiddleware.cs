using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndustries.Sellify.Core.Audit;
using ProjectIndustries.Sellify.Core.Audit.Services;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Audit
{
  public interface IIdentityProvider
  {
    string? GetCurrentIdentity();
  }

  public class AuditMiddleware
  {
    private readonly RequestDelegate _next;

    public AuditMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      var identityProvider = context.RequestServices.GetRequiredService<IIdentityProvider>();
      var labelProvider = context.RequestServices.GetRequiredService<IChangeSetLabelProvider>();
      ChangeSet? changeSet = null;
      var label = labelProvider.GetLabel();
      var identity = identityProvider.GetCurrentIdentity();
      if (!string.IsNullOrEmpty(label) && !string.IsNullOrEmpty(identity))
      {
        var mutableChangeSetProvider = context.RequestServices.GetRequiredService<IMutableChangeSetProvider>();

        changeSet = new ChangeSet(label, identity);
        mutableChangeSetProvider.SetChangeSet(changeSet);
      }

      await _next(context);
      if (changeSet != null && !changeSet.IsEmpty())
      {
        var ctx = context.RequestServices.GetRequiredService<DbContext>();
        ctx.Add(changeSet);
        await ctx.SaveChangesAsync();
      }
    }
  }
}