using System.Threading.Tasks;

namespace ProjectIndustries.Sellify.Infra
{
  public interface IDataSeeder
  {
    int Order { get; }

    Task SeedAsync();
  }
}