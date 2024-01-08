using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectIndustries.Sellify.App.Identity.Domain;
using ProjectIndustries.Sellify.Infra;

[assembly: HostingStartup(typeof(ProjectIndustries.Sellify.WebApi.Areas.Identity.IdentityHostingStartup))]
namespace ProjectIndustries.Sellify.WebApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}