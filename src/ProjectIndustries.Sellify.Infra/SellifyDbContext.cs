using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Extensions;
using IdentityServer4.EntityFramework.Interfaces;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProjectIndustries.Sellify.App.Identity.Domain;

namespace ProjectIndustries.Sellify.Infra
{
  public class SellifyDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IPersistedGrantDbContext
  {
    private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

    public SellifyDbContext(DbContextOptions<SellifyDbContext> options,
      IOptions<OperationalStoreOptions> operationalStoreOptions)
      : base(options)
    {
      _operationalStoreOptions = operationalStoreOptions;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
      modelBuilder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
      base.OnModelCreating(modelBuilder);
      modelBuilder.UseSnakeCaseNamingConvention();
    }


    /// <summary>
    /// Gets or sets the <see cref="DbSet{PersistedGrant}"/>.
    /// </summary>
    public DbSet<PersistedGrant> PersistedGrants { get; set; } = null!;

    /// <summary>
    /// Gets or sets the <see cref="DbSet{DeviceFlowCodes}"/>.
    /// </summary>
    public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; } = null!;

    Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();
  }
}