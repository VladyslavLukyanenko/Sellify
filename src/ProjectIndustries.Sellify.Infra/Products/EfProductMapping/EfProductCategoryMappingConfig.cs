using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.Core.Products;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.Products.EfProductMapping
{
  public class EfProductCategoryMappingConfig : SharedEntityMappingConfig<Category>
  {
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
      builder.HasOne<Category>()
        .WithMany()
        .HasForeignKey(_ => _.ParentCategoryId);

      base.Configure(builder);
    }
  }
}