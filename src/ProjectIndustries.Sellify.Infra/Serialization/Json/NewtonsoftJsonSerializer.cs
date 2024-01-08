using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProjectIndustries.Sellify.Core.Services;

namespace ProjectIndustries.Sellify.Infra.Serialization.Json
{
  public class NewtonsoftJsonSerializer : IJsonSerializer
  {
    public ValueTask<string> SerializeAsync<T>(T value, CancellationToken ct = default)
    {
      return ValueTask.FromResult(JsonConvert.SerializeObject(value));
    }

    public ValueTask<T> DeserializeAsync<T>(string raw, CancellationToken ct = default)
    {
      return ValueTask.FromResult(JsonConvert.DeserializeObject<T>(raw));
    }
  }
}