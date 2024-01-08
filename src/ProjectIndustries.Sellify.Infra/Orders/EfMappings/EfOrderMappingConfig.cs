using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.Core.Customers;
using ProjectIndustries.Sellify.Core.Orders;
using ProjectIndustries.Sellify.Core.Products;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.Orders.EfMappings
{
  public class EfOrderMappingConfig : SharedEntityMappingConfig<Order>
  {
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
      builder.HasOne<Customer>()
        .WithMany()
        .HasForeignKey(_ => _.CustomerId);

      builder.OwnsOne(_ => _.Product, txb =>
        {
          txb.HasOne<Product>()
            .WithMany()
            .HasForeignKey(r => r.Id);
        });

      builder.Property(_ => _.Metadata)
        .HasJsonConversion(json => FromJson<Dictionary<string, string>>(json), JsonSettings);

      // builder.OwnsOne(_ => _.ShippingAddress);
      // builder.OwnsOne(_ => _.BillingAddress);
      
      base.Configure(builder);
    }

    protected override void SetupIdGenerationStrategy(EntityTypeBuilder<Order> builder)
    {
      // no HiLo here
    }
  }
}