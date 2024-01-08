using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.Core.Analytics;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.Analytics.EfMappings
{
  public class UserSessionsMappingConfig : SharedEntityMappingConfig<UserSession>
  {
    public override void Configure(EntityTypeBuilder<UserSession> builder)
    {
      builder.HasOne<ApplicationUser>()
        .WithMany()
        .HasForeignKey(_ => _.UserId);

      base.Configure(builder);
    }

    protected override void SetupIdGenerationStrategy(EntityTypeBuilder<UserSession> builder)
    {
      // no need HiLo here
    }
  }
}