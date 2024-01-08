using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using ProjectIndustries.Sellify.Core.WebHooks;
using ProjectIndustries.Sellify.Core.WebHooks.Services;

namespace ProjectIndustries.Sellify.Infra.WebHooks.Services
{
  public class ReflectionBasedWebhookDataSerializer : IWebhookDataSerializer
  {
    public IEnumerable<KeyValuePair<string?, string?>> Serialize(WebHookDataBase data)
    {
      return Serialize((object) data);
    }

    private IEnumerable<KeyValuePair<string?, string?>> Serialize(object data)
    {
      var props = data.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
      if (props.Length == 0)
      {
        yield break;
      }

      foreach (var prop in props)
      {
        var propValue = prop.GetValue(data)!;
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        if (propValue is null)
        {
          yield return new KeyValuePair<string?, string?>(prop.Name, "");
          continue;
        }

        if (ReflectionHelper.IsDictionary(prop.PropertyType))
        {
          if (propValue is IEnumerable<KeyValuePair<string?, string?>> p)
          {
            foreach (var pair in p)
            {
              yield return pair;
            }
          }
        }
        else
        {
          var converter = TypeDescriptor.GetConverter(prop.PropertyType);
          if (converter != null!
              && converter is not CollectionConverter
              && converter.GetType() != typeof(TypeConverter))
          {
            var value = converter.ConvertToInvariantString(propValue);
            yield return new KeyValuePair<string?, string?>(prop.Name, value);
          }
          else
          {
            if (propValue is IEnumerable e)
            {
              foreach (var keyValuePair1 in SerializeEnumeration(e, prop))
              {
                yield return keyValuePair1;
              }
            }
            else
            {
              yield return new KeyValuePair<string?, string?>(prop.Name, null);
              foreach (var innerValue in Serialize(propValue))
              {
                yield return innerValue;
              }
            }
          }
        }
      }
    }

    private IEnumerable<KeyValuePair<string?, string?>> SerializeEnumeration(IEnumerable e, PropertyInfo prop)
    {
      var items = e.OfType<object>();
      if (!items.Any())
      {
        yield return new KeyValuePair<string?, string?>(prop.Name, "[]");
        yield break;
      }

      var itemConverter = TypeDescriptor.GetConverter(items.First().GetType());
      if (itemConverter.GetType() != typeof(TypeConverter))
      {
        var serializedItems = items.Select(item => itemConverter.ConvertToInvariantString(item));
        yield return new KeyValuePair<string?, string?>(prop.Name, string.Join(", ", serializedItems));
      }
      else
      {
        yield return new KeyValuePair<string?, string?>(prop.Name, null);
        foreach (var item in e)
        {
          foreach (var innerValue in Serialize(item))
          {
            yield return innerValue;
          }

          yield return new KeyValuePair<string?, string?>(null, null);
        }
      }
    }
  }
}