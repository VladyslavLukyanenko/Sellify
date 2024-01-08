using System.Linq;
using System.Text.Json.Serialization;
using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using IdentityServer4.Services;
using LinqToDB.EntityFrameworkCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using ProjectIndustries.Sellify.App.Config;
using ProjectIndustries.Sellify.App.Products.Config;
using ProjectIndustries.Sellify.Core.Primitives;
using ProjectIndustries.Sellify.Core.Stores.Config;
using ProjectIndustries.Sellify.Infra;
using ProjectIndustries.Sellify.Infra.Events;
using ProjectIndustries.Sellify.Infra.Serialization.Json;
using ProjectIndustries.Sellify.WebApi.Foundation.ActionResults;
using ProjectIndustries.Sellify.WebApi.Foundation.Config;
using ProjectIndustries.Sellify.WebApi.Foundation.Filters;
using ProjectIndustries.Sellify.WebApi.Foundation.FluentValidation;
using ProjectIndustries.Sellify.WebApi.Payments.Configs;
using Quartz;

namespace ProjectIndustries.Sellify.WebApi.Foundation
{
  // ReSharper disable once InconsistentNaming
  public static class IServiceCollectionExtensions
  {
    public static IServiceCollection InitializeConfiguration(this IServiceCollection services, IConfiguration cfg)
    {
      return services.Configure<JsonSerializerSettings>(ConfigureJsonSerializerSettings)
        .ConfigureCfgSectionAs<DataSourceConfig>(cfg, CfgSectionNames.DataSource)
        .ConfigureCfgSectionAs<CommonConfig>(cfg, CfgSectionNames.Common)
        .ConfigureCfgSectionAs<SsoConfig>(cfg, CfgSectionNames.Sso)
        .ConfigureCfgSectionAs<EfCoreConfig>(cfg, CfgSectionNames.EntityFramework)
        .ConfigureCfgSectionAs<ProductConfig>(cfg, CfgSectionNames.Product)
        .ConfigureCfgSectionAs<StripeGlobalConfig>(cfg, CfgSectionNames.Stripe)
        .ConfigureCfgSectionAs<PayPalGlobalConfig>(cfg, CfgSectionNames.PayPal)
        .ConfigureCfgSectionAs<SkrillGlobalConfig>(cfg, CfgSectionNames.Skrill)
        .ConfigureCfgSectionAs<StoresConfig>(cfg, CfgSectionNames.Stores);
    }

    public static IServiceCollection ConfigureCfgSectionAs<T>(this IServiceCollection svc, IConfiguration cfg,
      string sectionName) where T : class
    {
      var section = cfg.GetSection(sectionName);
      svc.Configure<T>(section);
      T c = section.Get<T>();
      svc.AddSingleton(c);

      return svc;
    }

    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration cfg)
    {
      var migrationsAssemblyName = typeof(SellifyDbContext).Assembly.GetName().Name;
      services.AddDbContext<SellifyDbContext>((svc, options) =>
        {
          var dataSourceConfig = cfg.GetSection(CfgSectionNames.DataSource).Get<DataSourceConfig>();
          options.UseNpgsql(dataSourceConfig.PostgresConnectionString,
            builder =>
            {
              builder.MigrationsAssembly(migrationsAssemblyName)
                .UseNodaTime();
            });

          options.EnableSensitiveDataLogging()
            .AddInterceptors(svc.GetRequiredService<EfInterceptorEventDispatcher>())
            /*.AddInterceptors(svc.GetRequiredService<SecondLevelCacheInterceptor>())*/;
        })
        .AddLogging()
        /*.AddMemoryCache()*/;

      services.AddScoped<DbContext>(s => s.GetRequiredService<SellifyDbContext>());
      services.AddScoped<IUnitOfWork, DbContextUnitOfWork>();
      
      LinqToDBForEFTools.Initialize();

      return services;
    }

    public static IServiceCollection AddConfiguredRateLimiter(this IServiceCollection services, IConfiguration cfg)
    {
      services.Configure<ClientRateLimitOptions>(cfg.GetSection("ClientRateLimiting"));
      services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
      services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

      return services;
    }

    public static IServiceCollection AddConfiguredCors(this IServiceCollection services, IConfiguration cfg)
    {
      return services.AddCors(options =>
      {
        options.AddPolicy("DefaultCors", policy =>
        {
          var conf = cfg.GetSection(CfgSectionNames.Common).Get<CommonConfig>();
          policy.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()

            // NOTICE: not allowed "any (*)" origin with credentials
            .WithOrigins(conf.Cors.AllowedHosts.ToArray());
        });
      });
    }

    public static IServiceCollection AddIdentityServerConfiguredCors(this IServiceCollection services,
      IConfiguration cfg)
    {
      services.AddSingleton<ICorsPolicyService>(s =>
      {
        var loggerFactory = s.GetService<ILoggerFactory>();
        var conf = cfg.GetSection(CfgSectionNames.Common).Get<CommonConfig>();
        var cors = new DefaultCorsPolicyService(loggerFactory.CreateLogger<DefaultCorsPolicyService>());
        foreach (var host in conf.Cors.AllowedHosts)
        {
          cors.AllowedOrigins.Add(host);
        }

        return cors;
      });

      return services;
    }

    public static IServiceCollection AddConfiguredMvc(this IServiceCollection services)
    {
      services.AddSingleton<ExpandPictureJsonConverter>();
      services
        .AddSingleton<IConfigureOptions<MvcNewtonsoftJsonOptions>, MvcNewtonsoftJsonConfigurer>()
        .AddRouting(options =>
        {
          options.LowercaseUrls = true;
          // options.LowercaseQueryStrings = true;
        })
        .AddControllers(_ =>
        {
          _.Filters.Add<HttpGlobalExceptionFilter>();
          _.Filters.Add<TransactionScopeFilter>(int.MaxValue);
        })
        .AddFluentValidation(_ =>
        {
          _.RegisterValidatorsFromAssembly(typeof(IServiceCollectionExtensions).Assembly);
          _.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
        })
        .AddJsonOptions(_ => { _.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
        .Services
        .AddRazorPages()
        .ConfigureApiBehaviorOptions(_ =>
        {
          _.InvalidModelStateResponseFactory = ctx =>
            new ValidationErrorResult(ctx.ModelState.Keys, ctx.HttpContext.Request.Path);
        })
        .AddViewLocalization(_ => _.ResourcesPath = "Resources")
        .AddNewtonsoftJson()
        .SetCompatibilityVersion(CompatibilityVersion.Latest);
      
      services.AddMemoryCache()
        .AddResponseCompression();

      services.AddTransient<IValidatorInterceptor, ErrorCodesPopulatorValidatorInterceptor>();

      return services;
    }

    private class MvcNewtonsoftJsonConfigurer : IConfigureOptions<MvcNewtonsoftJsonOptions>
    {
      private readonly ExpandPictureJsonConverter _expandPictureJsonConverter;

      public MvcNewtonsoftJsonConfigurer(ExpandPictureJsonConverter expandPictureJsonConverter)
      {
        _expandPictureJsonConverter = expandPictureJsonConverter;
      }

      public void Configure(MvcNewtonsoftJsonOptions options)
      {
        options.AllowInputFormatterExceptionMessages = true;
        NewtonsoftJsonSettingsFactory.ConfigureSettingsWithDefaults(options.SerializerSettings);
        options.SerializerSettings.Converters.Insert(0, _expandPictureJsonConverter);
      }
    }


    public static IServiceCollection AddConfiguredSignalR(this IServiceCollection services)
    {
      services.AddSignalR(options =>
        {
          // configure here...
        })
        .AddNewtonsoftJsonProtocol();

      return services;
    }

    public static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services)
    {
      services.AddMassTransit(_ =>
        {
          // _.AddConsumer<LicenseKeyAssociationChangeConsumer>();
          _.SetKebabCaseEndpointNameFormatter();
          _.UsingRabbitMq((ctx, c) =>
          {
            // var schedulerConfig = ctx.GetService<SchedulerConfig>();
            c.ConfigureEndpoints(ctx);
            // c.UseInMemoryScheduler(); //schedulerConfig.QueueName);
          });
        })
        .AddMassTransitHostedService();

      return services;
    }

    public static IServiceCollection AddConfiguredQuartz(this IServiceCollection services, IConfiguration cfg)
    {
      services.Configure<QuartzOptions>(cfg.GetSection("Quartz"));
      services.AddQuartz(_ =>
      {
        _.UseMicrosoftDependencyInjectionScopedJobFactory(c =>
        {
          c.CreateScope = true;
          c.AllowDefaultConstructor = true;
        });
      });

      services.AddQuartzServer(_ => { _.WaitForJobsToComplete = true; });
      return services;
    }

    public static IServiceCollection AddConfiguredAuthentication(this IServiceCollection services, IConfiguration cfg)
    {
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddOpenIdConnect("Google", "Google", _ =>
        {
          var gc = cfg.GetSection("Authentication:Google");
          _.ClientId = gc["ClientId"];
          _.ClientSecret = gc["ClientSecret"];
          _.Authority = "https://accounts.google.com";
          _.ResponseType = OpenIdConnectResponseType.Code;
          _.CallbackPath = "/signin-google";
        })
        .AddIdentityServerJwt();
      
      

      return services;
    }

    private static void ConfigureJsonSerializerSettings(JsonSerializerSettings serializerSettings)
    {
      NewtonsoftJsonSettingsFactory.ConfigureSettingsWithDefaults(serializerSettings);
    }

    private static class CfgSectionNames
    {
      public const string DataSource = nameof(DataSource);
      public const string Common = nameof(Common);
      public const string Sso = nameof(Sso);
      public const string EntityFramework = nameof(EntityFramework);
      public const string Stores = nameof(Stores);
      public const string Product = nameof(Product);
      public const string Stripe = nameof(Stripe);
      public const string PayPal = nameof(PayPal);
      public const string Skrill = nameof(Skrill);
    }
  }
}