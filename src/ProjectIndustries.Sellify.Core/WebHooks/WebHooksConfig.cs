using System;
using CSharpFunctionalExtensions;

namespace ProjectIndustries.Sellify.Core.WebHooks
{
  public class WebHooksConfig : Entity, IStoreBoundEntity
  {
    public Guid StoreId { get; set; }
    public bool IsEnabled { get; set; }
    public string ClientSecret { get; set; } = null!;
  }
}