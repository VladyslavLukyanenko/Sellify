using AspNetCoreRateLimit;
using Autofac;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PayPal.Api;
using PayPalCheckoutSdk.Core;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.Infra;
using ProjectIndustries.Sellify.Infra.Stores.EventHandlers;
using ProjectIndustries.Sellify.Infra.WebHooks;
using ProjectIndustries.Sellify.Infra.WebHooks.Consumers;
using ProjectIndustries.Sellify.WebApi.Foundation;
using ProjectIndustries.Sellify.WebApi.Foundation.Composition.Proxies;
using ProjectIndustries.Sellify.WebApi.Foundation.Services;
using ProjectIndustries.Sellify.WebApi.Foundation.SwaggerSupport.Swashbuckle;
using ProjectIndustries.Sellify.WebApi.Payments.Configs;

namespace ProjectIndustries.Sellify.WebApi
{
  public class Startup
  {
    private const string ApiVersion = "v1";
    private const string ApiTitle = "Sellify API";
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public Startup(IConfiguration configuration, IHostEnvironment environment)
    {
      _configuration = configuration;
      _environment = environment;
    }

    public void ConfigureContainer(ContainerBuilder builder)
    {
      builder.RegisterAssemblyModules(GetType().Assembly);
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      // const string providerName = "GlobalInMemoryEfCoreCache";
      // services.AddEFSecondLevelCache(o => o.UseEasyCachingCoreProvider(providerName)
      //     .DisableLogging(!_environment.IsDevelopment())
      //   /*.CacheAllQueries(CacheExpirationMode.Sliding, TimeSpan.FromSeconds(15))*/);
      //
      // services.AddEasyCaching(o =>
      // {
      //   o.UseInMemory(c => { c.DBConfig = new InMemoryCachingOptions(); }, providerName);
      // });
      services.InitializeConfiguration(_configuration)
        .AddApplicationDbContext(_configuration)
        .AddConfiguredRateLimiter(_configuration)
        .AddConfiguredCors(_configuration)
        .AddConfiguredMvc()
        .AddConfiguredSignalR()
        .AddConfiguredSwagger(ApiVersion, ApiTitle);

      services.AddSingleton(ctx =>
      {
        var config = ctx.GetRequiredService<PayPalGlobalConfig>();
        PayPalEnvironment env = _environment.IsDevelopment()
          ? new SandboxEnvironment(config.ClientId, config.Secret)
          : new LiveEnvironment(config.ClientId, config.Secret);

        return env;
      });

      services.AddSingleton(ctx =>
      {
        var env = ctx.GetRequiredService<PayPalEnvironment>();
        return new PayPalHttpClient(env);
      });

      services.AddSingleton(ctx =>
      {
        var config = ctx.GetRequiredService<PayPalGlobalConfig>();
        return new OAuthTokenCredential(config.ClientId, config.Secret);
      });

      services.AddAuthorization(_ => { });
      services.AddDefaultIdentity<ApplicationUser>()
        .AddRoles<ApplicationRole>()
        .AddEntityFrameworkStores<SellifyDbContext>()
        .AddClaimsPrincipalFactory<EnrichedUserClaimsPrincipalFactory>();

      services.AddIdentityServer()
        .AddProfileService<ProfileService>()
        .AddApiAuthorization<ApplicationUser, SellifyDbContext>();
      services.AddIdentityServerConfiguredCors(_configuration);
      services.AddConfiguredAuthentication(_configuration);


      services.AddScoped<WebHookPayloadsPublisher>();
      services.AddSingleton(typeof(SingletonToScopedTransitionObserver<>));


      // RateLimiting
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
      // services.Add


      services.AddMassTransit(_ =>
        {
          _.AddConsumer<CreateStoreOnApplicationUserCreated>();
          _.AddConsumer<WebHookSenderConsumer>();
          _.SetKebabCaseEndpointNameFormatter();
          _.UsingRabbitMq((ctx, c) =>
          {
            // var schedulerConfig = ctx.GetService<SchedulerConfig>();
            c.ConfigureEndpoints(ctx);

            //
            var webhookObserver =
              ctx.GetRequiredService<SingletonToScopedTransitionObserver<WebHookPayloadsPublisher>>();
            c.ConnectPublishObserver(webhookObserver);
            c.ConnectSendObserver(webhookObserver);
            // c.UseInMemoryScheduler(); //schedulerConfig.QueueName);
          });
        })
        .AddMassTransitHostedService();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      // RateLimiting
      app.UseClientRateLimiting();
      app.UseResponseCompression();

      // app.UseConfiguredApm(_configuration);
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      var corsConfig = app.UseCommonHttpBehavior(env);
      app.UseStaticFiles();
      app.UseRouting();
      app.UseConfiguredCors(corsConfig);

      app.UseAuthentication();
      app.UseAuthorization();
      app.UseIdentityServer();

      app.UseAudit();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapRazorPages();
      });

      app.UseConfiguredSwagger(ApiVersion, ApiTitle);
    }
  }
}