using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ProjectIndustries.Sellify.App.Config;
using ProjectIndustries.Sellify.WebApi.Foundation.Audit;
using ProjectIndustries.Sellify.WebApi.Foundation.Config;

namespace ProjectIndustries.Sellify.WebApi.Foundation
{
  public static class ApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseConfiguredSwagger(this IApplicationBuilder app, string apiVersion,
      string apiTitle)
    {
      app.UseSwagger(c => { });
      app.UseSwaggerUI(c => { c.SwaggerEndpoint($"/swagger/{apiVersion}/swagger.json", apiTitle); });
      return app;
    }

    public static CommonConfig UseCommonHttpBehavior(this IApplicationBuilder app, IWebHostEnvironment env)
    {
      var startupConfig = app.ApplicationServices.GetRequiredService<IOptions<StartupConfiguration>>().Value;

      if (!env.IsDevelopment())
      {
        app.UseHsts();
      }

      if (startupConfig.UseHttps)
      {
        app.UseHttpsRedirection();
      }

      app.UseForwardedHeaders(new ForwardedHeadersOptions
      {
        ForwardedHeaders = ForwardedHeaders.All
      });

      return app.ApplicationServices.GetRequiredService<CommonConfig>();
    }

    public static IApplicationBuilder UseConfiguredCors(this IApplicationBuilder app,
      CommonConfig config)
    {
      if (config.Cors.UseCors)
      {
        app.UseCors("DefaultCors");
      }

      return app;
    }

    public static IApplicationBuilder UseConfiguredApm(this IApplicationBuilder app, IConfiguration configuration)
    {
      app.UseAllElasticApm(configuration);

      return app;
    }

    public static IApplicationBuilder UseAudit(this IApplicationBuilder app)
    {
      app.UseMiddleware<AuditMiddleware>();

      return app;
    }
  }
}