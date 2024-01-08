using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectIndustries.Sellify.Infra.Serialization.Json;
using ProjectIndustries.Sellify.WebApi.Foundation;
using Serilog;

namespace ProjectIndustries.Sellify.WebApi
{
  public class Program
  {
    private const string ProductionEnv = "Production";

    static Program()
    {
      JsonConvert.DefaultSettings = NewtonsoftJsonSettingsFactory.CreateSettingsProvider();
      var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? ProductionEnv;

      var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("serilogsettings.json", false, true)
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{environmentName}.json", true)
        .AddJsonFile("appsettings.local.json", true)
        .AddEnvironmentVariables();

      var args = Environment.GetCommandLineArgs();
      if (args.Any())
      {
        builder.AddCommandLine(args);
      }

      Configuration = builder.Build();
      var loggerConfiguration = new LoggerConfiguration()
        // .Enrich.WithElasticApmCorrelationInfo()
        .ReadFrom.Configuration(Configuration);
      //
      // var esSinkCfg = Configuration.GetSection("ElasticSearchSink").Get<ElasticSearchSinkConfig>();
      // if (esSinkCfg.IsEnabled)
      // {
      //   var sinkOptions = new ElasticsearchSinkOptions(new CloudConnectionPool(esSinkCfg.CloudId,
      //     new BasicAuthenticationCredentials(esSinkCfg.Login, esSinkCfg.Password)))
      //   {
      //     IndexFormat = "sellify-{0:yyyyMMdd}",
      //     AutoRegisterTemplate = true,
      //     CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true, inlineFields: true),
      //     EmitEventFailure =
      //       EmitEventFailureHandling.WriteToSelfLog |
      //       EmitEventFailureHandling.RaiseCallback |
      //       EmitEventFailureHandling.ThrowException,
      //     FailureCallback = e => { Console.WriteLine("Unable to submit event " + e.MessageTemplate); }
      //   };
      //
      //   loggerConfiguration.WriteTo.Elasticsearch(sinkOptions);
      // }

      Log.Logger = loggerConfiguration.CreateLogger();
    }

    private static IConfigurationRoot Configuration { get; }

    public static async Task Main(string[] args)
    {
      var logger = Log.ForContext(typeof(Program));
      try
      {
        logger.Information("Starting application");

        var webHost = CreateHostBuilder(args).Build();

        await webHost.MigrateDatabaseIfAllowedAsync();
        await webHost.SeedRateLimiterPolicyStoreAsync();
        // await webHost.SetupQuartzStoreAsync();

        await webHost.RunAsync();
      }
      catch (Exception e)
      {
        logger.Fatal(e, "Host terminated unexpectedly");
        throw;
      }
      finally
      {
        Log.CloseAndFlush();
      }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
      return Host.CreateDefaultBuilder(args)
        .UseServiceProviderFactory(new AutofacServiceProviderFactory())
        .ConfigureHostConfiguration(c => c.AddConfiguration(Configuration))
        .ConfigureAppConfiguration(c => c.AddConfiguration(Configuration))
        .ConfigureWebHostDefaults(configure =>
        {
          configure
            .ConfigureServices(container => { container.AddAutofac(); })
            .UseStartup<Startup>();
        })
        .ConfigureLogging((ctx, logging) =>
        {
          logging.ClearProviders();
          logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));
          logging.AddSerilog();
        })
        .UseConsoleLifetime();
    }
  }
}