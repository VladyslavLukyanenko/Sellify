using Autofac;
using ProjectIndustries.Sellify.App.Services;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Composition
{
  public class AppModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterAssemblyTypes(typeof(Entity).Assembly)
        .Where(_ => _.Namespace?.Contains(".Services") ?? false)
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

      builder.RegisterAssemblyTypes(typeof(IPathsService).Assembly)
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

      builder.RegisterAssemblyTypes(GetType().Assembly)
        // .Except<MvcPermissionsRegistry>()
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();
      //
      // builder.RegisterType<MvcPermissionsRegistry>()
      //   .AsImplementedInterfaces()
      //   .SingleInstance();
    }
  }
}