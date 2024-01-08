using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.Core.Products;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.Products.EfProductMapping
{
  public class EfProductMappingConfig : SharedEntityMappingConfig<Product>
  {
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
      builder.Property(_ => _.Attributes)
        .UsePropertyAccessMode(PropertyAccessMode.Field)
        .HasJsonConversion(FromJson<List<ProductAttribute>>, JsonSettings);

      builder.HasOne<Category>()
        .WithMany()
        .HasForeignKey(_ => _.CategoryId);
      
      base.Configure(builder);
    }
  }
}