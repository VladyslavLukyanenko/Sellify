using System;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Analytics;
using ProjectIndustries.Sellify.Core.Analytics.Services;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Infra.Repositories;

namespace ProjectIndustries.Sellify.Infra.Analytics.Services
{
  public class EfUserSessionRepository : EfCrudRepository<UserSession, Guid>, IUserSessionRepository
  {
    public EfUserSessionRepository(DbContext context, IUnitOfWork unitOfWork)
      : base(context, unitOfWork)
    {
    }
  }
}