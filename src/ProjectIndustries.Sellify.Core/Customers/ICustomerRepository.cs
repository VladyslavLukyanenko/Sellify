using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Core.Customers
{
  public interface ICustomerRepository
  {
    ValueTask<Customer?> GetByEmailAsync(Guid storeId, string email, CancellationToken ct = default);
    ValueTask<Customer> CreateAsync(Customer customer, CancellationToken ct = default);
    Customer Update(Customer customer);
  }
}