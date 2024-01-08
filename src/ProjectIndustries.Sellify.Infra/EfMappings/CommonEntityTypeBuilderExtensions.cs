using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using ProjectIndustries.Sellify.Core.Primitives;

namespace ProjectIndustries.Sellify.Infra.EfMappings
{
  public static class CommonEntityTypeBuilderExtensions
  {
    public static EntityTypeBuilder<T> SetPrivatePropertyFieldAccessMode<T>(this EntityTypeBuilder<T> builder,
      string propName)
      where T : class
    {
      var navigation = builder.Metadata.FindNavigation(propName);
      navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

      return builder;
    }

    public static PropertyBuilder<TEnum> HasEnumerationConversion<TEnum>(this PropertyBuilder<TEnum> self)
      where TEnum : Enumeration
    {
      return self.HasConversion(new ValueConverter<TEnum, int>(_ => _.Id,
        value => value.ToEnumeration<TEnum>()));
    }

    public static PropertyBuilder<TSrc> HasJsonConversion<TSrc, TResult>(this PropertyBuilder<TSrc> self,
      Func<string, TResult?> deserializer, JsonSerializerSettings settings)
      where TResult: TSrc
    {
      self.HasColumnType("jsonb")
        .HasConversion(
          new ValueConverter<TSrc, string>(item => JsonConvert.SerializeObject(item, settings),
            json => deserializer(json)!));

      return self;
    }

    public static PropertyBuilder<TEnum> HasNullableEnumerationConversion<TEnum>(this PropertyBuilder<TEnum> self)
      where TEnum : Enumeration?
    {
      return self.HasConversion(new ValueConverter<TEnum, int?>(_ => _!.Id,
        value => value.HasValue ? value.Value.ToEnumeration<TEnum>() : null!));
    }
  }
}