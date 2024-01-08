using System;
using System.Reflection;
using Newtonsoft.Json;
using ProjectIndustries.Sellify.App.Model;
using ProjectIndustries.Sellify.App.Services;

namespace ProjectIndustries.Sellify.Infra.Serialization.Json
{
  public class ExpandPictureJsonConverter : JsonConverter
  {
    private readonly IPathsService _pathsService;

    public ExpandPictureJsonConverter(IPathsService pathsService)
    {
      _pathsService = pathsService;
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
      var absoluteUrl = _pathsService.GetAbsoluteImageUrl(value as string, null);
      writer.WriteValue(absoluteUrl);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
      JsonSerializer serializer)
    {
      return reader.ReadAsString();
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(string)
                                                        && objectType.GetCustomAttribute<ExpandPictureAttribute>()
                                                          is not null;
  }
}