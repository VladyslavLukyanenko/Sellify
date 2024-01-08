using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace ProjectIndustries.Sellify.Infra.Serialization.Json
{
  public static class NewtonsoftJsonSettingsFactory
  {
    public static Func<JsonSerializerSettings> CreateSettingsProvider(JsonSerializerSettings? settings = null)
    {
      settings ??= JsonConvert.DefaultSettings?.Invoke() ?? new JsonSerializerSettings();
      ConfigureSettingsWithDefaults(settings);

      return SettingsProvider;

      JsonSerializerSettings SettingsProvider()
      {
        return settings;
      }
    }

    public static void ConfigureSettingsWithDefaults(JsonSerializerSettings settings)
    {
      settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
      settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
      settings.NullValueHandling = NullValueHandling.Ignore;
      settings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
      settings.DateParseHandling = DateParseHandling.DateTimeOffset;
      settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
      settings.Converters.Add(new StringEnumConverter());
      settings.Converters.Add(new IBinaryDataJsonConverter());
      settings.Converters.Add(new WebHookPayloadSerializer());

      settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
    }
  }
}