using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProjectIndustries.Sellify.Core.Customers;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Infra.Repositories;

namespace ProjectIndustries.Sellify.Infra.Customers.Services
{
  public class EfCustomerRepository : EfSoftRemovableCrudRepository<Customer, long>, ICustomerRepository
  {
    public EfCustomerRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }

    public async ValueTask<Customer?> GetByEmailAsync(Guid storeId, string email, CancellationToken ct = default)
    {
      var normalizedEmail = email.ToUpper();
      return await DataSource.FirstOrDefaultAsync(_ => _.Email.ToUpper() == normalizedEmail && _.StoreId == storeId,
        ct);
    }
  }
}