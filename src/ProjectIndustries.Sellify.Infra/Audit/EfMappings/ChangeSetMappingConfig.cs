using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.Core.Audit;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.Audit.EfMappings
{
  public class ChangeSetMappingConfig : EntityMappingConfig<ChangeSet>
  {
    protected override string SchemaName => "Audit";

    public override void Configure(EntityTypeBuilder<ChangeSet> builder)
    {
      /*builder.HasOne<Facility>()
        .WithMany()
        .HasForeignKey(_ => _.FacilityId)
        .IsRequired(false);*/

      builder.HasOne<ApplicationUser>()
        .WithMany()
        .HasForeignKey(_ => _.UpdatedBy);

      builder.OwnsMany(_ => _.Entries, ob =>
        {
          ob.HasKey(_ => _.Id);
          ob.Property(_ => _.Payload)
            .HasJsonConversion(FromJson<Dictionary<string, string?>>, JsonSettings);

          MappedToTableWithDefaults(ob);
        })
        .UsePropertyAccessMode(PropertyAccessMode.Field);

      base.Configure(builder);
    }

    protected override void SetupIdGenerationStrategy(EntityTypeBuilder<ChangeSet> builder)
    {
    }
  }
}