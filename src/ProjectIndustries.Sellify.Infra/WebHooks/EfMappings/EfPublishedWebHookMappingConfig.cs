using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.WebHooks.EfMappings
{
  public class EfPublishedWebHookMappingConfig : SharedEntityMappingConfig<PublishedWebHook>
  {
    public override void Configure(EntityTypeBuilder<PublishedWebHook> builder)
    {
      builder.Property(_ => _.Payload).HasColumnType("jsonb");
      base.Configure(builder);
    }
  }
}