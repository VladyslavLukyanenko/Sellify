using System.Collections.Generic;
using Autofac;
using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.StaticFiles;
using ProjectIndustries.Sellify.Infra;
using ProjectIndustries.Sellify.Infra.Events;

namespace ProjectIndustries.Sellify.WebApi.Foundation.Composition
{
  public class InfraModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<FileExtensionContentTypeProvider>().As<IContentTypeProvider>().InstancePerLifetimeScope();
      builder.RegisterAssemblyTypes(typeof(IDataSeeder).Assembly)
        .AsImplementedInterfaces()
        .InstancePerLifetimeScope();

      // registers bus consumers 
      builder.RegisterAssemblyTypes(typeof(SellifyDbContext).Assembly)
        .AsClosedTypesOf(typeof(IConsumer<>));

      builder.RegisterType<Mapper>().As<IMapper>().InstancePerLifetimeScope();
      builder.RegisterAssemblyTypes(typeof(EntitiesMappingProfile).Assembly)
        .Where(t => typeof(Profile).IsAssignableFrom(t))
        .As<Profile>()
        .SingleInstance();

      builder.RegisterType<EfInterceptorEventDispatcher>()
        .AsSelf();

      builder.Register<IConfigurationProvider>(ctx =>
        {
          var mapperConfig = new MapperConfiguration(cfg =>
          {
            var profiles = ctx.Resolve<IEnumerable<Profile>>();
            foreach (var profile in profiles)
            {
              cfg.AddProfile(profile);
            }

            cfg.DisableConstructorMapping();
          });

          mapperConfig.AssertConfigurationIsValid();
          return mapperConfig;
        })
        .SingleInstance();
    }
  }
}