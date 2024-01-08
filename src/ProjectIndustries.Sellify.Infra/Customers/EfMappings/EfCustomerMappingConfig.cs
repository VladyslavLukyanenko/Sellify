using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.Core.Customers;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.Customers.EfMappings
{
  public class EfCustomerMappingConfig : SharedEntityMappingConfig<Customer>
  {
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
      builder.HasIndex(_ => new {_.StoreId, _.Email}).IsUnique();
      base.Configure(builder);
    }
  }
}