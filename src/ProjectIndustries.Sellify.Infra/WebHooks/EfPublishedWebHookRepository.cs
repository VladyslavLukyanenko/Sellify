using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;
using ProjectIndustries.Sellify.Infra.Repositories;

namespace ProjectIndustries.Sellify.Infra.WebHooks
{
  public class EfPublishedWebHookRepository : EfCrudRepository<PublishedWebHook>, IPublishedWebHookRepository
  {
    public EfPublishedWebHookRepository(DbContext context, IUnitOfWork unitOfWork)
      : base(context, unitOfWork)
    {
    }
  }
}