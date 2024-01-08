using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Infra.EfMappings;

namespace ProjectIndustries.Sellify.Infra.WebHooks.EfMappings
{
  public class EfWebHookBindingMappingConfig : SharedEntityMappingConfig<WebHookBinding>
  {
    public override void Configure(EntityTypeBuilder<WebHookBinding> builder)
    {
      builder.Property(_ => _.EventType).IsRequired();
      builder.Property(_ => _.ListenerEndpoint).IsRequired();
      builder.Property(_ => _.ReceiverType)
        .HasEnumerationConversion();

      base.Configure(builder);
    }
  }
}