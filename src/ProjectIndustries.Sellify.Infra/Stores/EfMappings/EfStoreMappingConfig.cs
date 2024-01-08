using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.Core.Stores;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.Stores.EfMappings
{
  public class EfStoreMappingConfig : SharedEntityMappingConfig<Store>
  {
    public override void Configure(EntityTypeBuilder<Store> builder)
    {
      builder.HasOne<ApplicationUser>()
        .WithMany()
        .HasForeignKey(_ => _.OwnerId);

      builder.OwnsOne(_ => _.HostingConfig);

      builder.OwnsOne(_ => _.PaymentGatewayConfigs, pgb =>
      {
        pgb.OwnsOne(_ => _.Skrill);
        pgb.OwnsOne(_ => _.Stripe);
        pgb.OwnsOne(_ => _.PayPal);
      });

      base.Configure(builder);
    }

    protected override void SetupIdGenerationStrategy(EntityTypeBuilder<Store> builder)
    {
      // no need for HiLo here
    }
  }
}