using System.Collections.Generic;
using System.Linq;
using Autofac;
using ProjectIndustries.Sellify.App.Audit.Services;
using ProjectIndustries.Sellify.Core.Audit.Mappings;
using ProjectIndustries.Sellify.Core.Audit.Processors;
using ProjectIndustries.Sellify.Core.Audit.Services;
using ProjectIndustries.Sellify.Infra.Audit.Providers;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Composition
{
  public class AuditModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(typeof(ICurrentChangeSetProvider).Assembly)
        .AsImplementedInterfaces()
        .InNamespaceOf<MutableCurrentChangeSetProvider>()
        .InstancePerLifetimeScope();

      builder.RegisterAssemblyTypes(typeof(ChangeSetEntryPayloadMapper).Assembly)
        .AsImplementedInterfaces()
        .InNamespaceOf<ChangeSetEntryPayloadMapper>()
        .InstancePerLifetimeScope();

      builder.RegisterAssemblyTypes(typeof(ChangeSetProvider).Assembly)
        .InNamespaceOf<ChangeSetProvider>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

      builder.RegisterAssemblyTypes(typeof(IAuditingEntityPreProcessor).Assembly)
        .InNamespaceOf<IAuditingEntityPreProcessor>()
        .AsSelf()
        .InstancePerLifetimeScope();

      builder.RegisterAssemblyTypes(typeof(ChangeSetProvider).Assembly)
        .Where(t => typeof(IChangeSetEntryValueConverter).IsAssignableFrom(t))
        .AsSelf()
        .InstancePerLifetimeScope();

      builder.Register<IEnumerable<IEntityToChangeSetEntryMapper>>(ctx =>
        {
          var providers = ctx.Resolve<IEnumerable<IAuditMappingProvider>>();
          var factory = ctx.Resolve<IEntityToChangeSetEntryMapperFactory>();
          return providers.SelectMany(_ => _.Builders.Values).Select(b => factory.Create(b)).ToArray();
        })
        .SingleInstance();
    }
  }
}